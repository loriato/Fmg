using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.InteropServices;

namespace Europa.Commons.LDAP
{
    public class LdapAuth
    {
        private string _path;

        public LdapAuth(string path)
        {
            _path = path;
        }

        public LdapModel Autenticate(string domain, string username, string pwd)
        {
            string domainAndUsername = domain + @"\" + username;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(&(objectClass=user)(sAMAccountName=" + username + "))";
                // specify which property values to return in the search
                search.PropertiesToLoad.Add("givenName");   // first name
                search.PropertiesToLoad.Add("sn");          // last name
                search.PropertiesToLoad.Add("mail");        // smtp mail address
                search.PropertiesToLoad.Add("MemberOf");        // smtp mail address

                // perform the search
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return null;
                }

                LdapModel model = new LdapModel();
                if (result.Properties["givenName"].Count > 0)
                {
                    model.Nome = (string)result.Properties["givenName"][0];
                }
                if (result.Properties["sn"].Count > 0)
                {
                    model.Sobrenome = (string)result.Properties["sn"][0];
                }
                if (result.Properties["mail"].Count > 0)
                {
                    model.Email = (string)result.Properties["mail"][0];
                }
                if (result.Properties["MemberOf"].Count > 0)
                {
                    model.GruposActiveDiretory = GetGroupNames(result);
                }
                model.Login = username;

                return model;
            }
            catch (COMException)
            {
                return null;
            }
        }

        private static List<string> GetGroupNames(SearchResult result)
        {
            List<string> groupNames = new List<string>();
            int propertyCount = result.Properties["memberOf"].Count;
            int equalsIndex, commaIndex;

            string dn = "";

            for (int propertyCounter = 0; propertyCounter < propertyCount;
                propertyCounter++)
            {
                dn = (string)result.Properties["memberOf"][propertyCounter];

                equalsIndex = dn.IndexOf("=", 1);
                commaIndex = dn.IndexOf(",", 1);
                if (-1 == equalsIndex)
                {
                    return new List<string>();
                }
                groupNames.Add(dn.Substring((equalsIndex + 1),
                            (commaIndex - equalsIndex) - 1));
            }

            // Garantindo que todos estejam em maiúsculo
            groupNames = groupNames.Select(reg => reg.ToUpper()).ToList();

            return groupNames;
        }
    }
}
