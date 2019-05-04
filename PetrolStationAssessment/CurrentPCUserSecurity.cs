using System.Security.AccessControl;
using System.Security.Principal;
using System.IO;
// Refered to: https://stackoverflow.com/questions/1281620/checking-for-directory-and-file-write-permissions-in-net/5503648

namespace PetrolStationAssessment
{
    class CurrentPCUserSecurity
    {
        //Fields
        WindowsIdentity _currentUser;
        WindowsPrincipal _currentPrincipal;

        //Constructor
        public CurrentPCUserSecurity()
        {
            _currentUser = WindowsIdentity.GetCurrent();
            _currentPrincipal = new WindowsPrincipal(_currentUser);
        }

        /// <summary>
        /// Get the collection of authorization rules that apply to the directory for further 
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool HasAccess(DirectoryInfo directory, FileSystemRights right)
        {
            //Check if directory path exists
            if (!directory.Exists)
            {
                return false;
            }
            // Get the collection of authorization rules that apply to the directory.
            AuthorizationRuleCollection acl = directory.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
            return HasDirectoryAccess(right, acl);
        }

        /// <summary>
        /// Check if directory authorisation rules allow refered right to be processed.
        /// </summary>
        /// <param name="right"></param>
        /// <param name="acl"></param>
        /// <returns>Boolean value indicating whether access allowed or not </returns>
        private bool HasDirectoryAccess(FileSystemRights right, AuthorizationRuleCollection acl)
        {
            bool allow = false;
            bool inheritedAllow = false;
            bool inheritedDeny = false;

            for (int i = 0; i < acl.Count; i++)
            {
                var currentRule = (FileSystemAccessRule)acl[i];
                // If the current rule applies to the current user.
                if (_currentUser.User.Equals(currentRule.IdentityReference) ||
                    _currentPrincipal.IsInRole((SecurityIdentifier)currentRule.IdentityReference))
                {                 
                    if (currentRule.AccessControlType.Equals(AccessControlType.Deny))
                    {
                        if ((currentRule.FileSystemRights & right) == right)
                        {
                            if (currentRule.IsInherited)
                            {
                                inheritedDeny = true;
                            }
                            else
                            { // Non inherited "deny" takes overall precedence.
                                return false;
                            }
                        }
                    }
                    else if (currentRule.AccessControlType.Equals(AccessControlType.Allow))
                    {
                        if ((currentRule.FileSystemRights & right) == right)
                        {
                            if (currentRule.IsInherited)
                            {
                                inheritedAllow = true;
                            }
                            else
                            {
                                allow = true;
                            }
                        }
                    }
                }
            }

            if (allow)
            { // Non inherited "allow" takes precedence over inherited rules.
                return true;
            }
            return inheritedAllow && !inheritedDeny;
        }
    }
}
