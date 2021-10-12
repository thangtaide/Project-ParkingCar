using System;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class CheckOutDal
    {
        public static int ERROR = -1;
        public static int WRONG = 0;
        public static int EXIST = 1;
        public int CheckOutLicensePlate(int id, string licensePlate){
            int licensePlateReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "select * from CheckInOut where  card_id = '"+id+"' and vehicle_number = '"+licensePlate+"' and checkout_time is null;";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                if(reader.Read()){
                    licensePlateReturn = reader.GetInt32("inout_id");
                } else {
                    licensePlateReturn = WRONG;
                }
                reader.Close();
                connection.Close();
            }catch{
                licensePlateReturn = ERROR;
            }
            return licensePlateReturn;
        }
        public int SaveCheckOutTime(int cardId, int UserId)
        {
            int SaveReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "update CheckInOut set checkout_time=CURRENT_TIMESTAMP(), checkout_user_id='"+UserId+"' where card_id='"+cardId+"';";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                SaveReturn = 1;
                connection.Close();
            }catch{
                SaveReturn = ERROR;
            }
            return SaveReturn;
        }
        public int SaveCheckOut(int inOutId, int price)
        {
            int SaveReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "update CheckInOut set total='"+price+"' where inout_id='"+inOutId+"';";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                SaveReturn = 1;
                connection.Close();
            }catch{
                SaveReturn = ERROR;
            }
            return SaveReturn;
        }
    }
}