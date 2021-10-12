using System;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class UserDal
    {
        public static int USER_ERROR = -1;
        public static int ACCOUNT_WRONG = 0;
        public static int ACCOUNT_EXIST = 1;
        // 0: account wrong
        // 1: ok
        // -1: can't connect to db or error
        public int Login(string userName, string password){
            int loginReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "select * from Guards where guard_account='"+userName+"' and guard_pass='"+Md5Algorithms.CreateMD5(password)+"';";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                // MySqlConnection connection = DbHelper.GetConnection();
                // connection.Open();
                // MySqlCommand command = connection.CreateCommand();
                // command.CommandText = "select * from Guards where guard_account='"+userName+"' and guard_pass='"+Md5Algorithms.CreateMD5(password)+"';";
                // MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read()){
                    loginReturn = reader.GetInt32("guard_id");
                } else {
                    loginReturn = ACCOUNT_WRONG;
                }
                reader.Close();
                connection.Close();
            }catch{
                loginReturn = USER_ERROR;
            }
            return loginReturn;
        }
    }
}
