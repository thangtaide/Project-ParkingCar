using System;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class CheckinDal
    {
        public static int ERROR = -1;
        public static int WRONG = 0;
        public static int EXIST = 1;
        // 0: wrong
        // 1: ok
        // -1: can't connect to db or error
        public int CheckCardID(int id){
            int cardIdReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "select * from ParkingCards where card_id='"+id+"';";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                if(reader.Read()){
                    cardIdReturn = EXIST;
                } else {
                    cardIdReturn = WRONG;
                }
                reader.Close();
                connection.Close();
            }catch{
                cardIdReturn = ERROR;
            }
            return cardIdReturn;
        }
        public int CheckLicensePlate(int id, string licensePlate){
            int licensePlateReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "select * from ParkingCards where card_id='"+id+"' and vehicle_number = '"+licensePlate+"';";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                if(reader.Read()){
                    licensePlateReturn = EXIST;
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
        
        public int CheckDailyCard(int id){
            int DailyCardReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "select * from ParkingCards where card_id='"+id+"' and customer_id is null;";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                if(reader.Read()){
                    DailyCardReturn = EXIST;
                } else {
                    DailyCardReturn = WRONG;
                }
                reader.Close();
                connection.Close();
            }catch{
                DailyCardReturn = ERROR;
            }
            return DailyCardReturn;
        }
        public int SaveLicensePlate(int id, string licensePlate)
        {
            int SaveReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "update ParkingCards set vehicle_number='"+licensePlate+"'where card_id='"+id+"';";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                SaveReturn = 1;
                connection.Close();
            }catch{
                SaveReturn = ERROR;
            }
            return SaveReturn;
        }
        public int CheckExpiryDate(int id){
            int ExpiryReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "select * from ParkingCards where card_id='"+id+"' and expiry_date > CURRENT_DATE();";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                if(reader.Read()){
                    ExpiryReturn = 1;// Not out date
                } else {
                    ExpiryReturn = 0;// Out Date
                }
                reader.Close();
                connection.Close();
            }catch{
                ExpiryReturn = ERROR;
            }
            return ExpiryReturn;
        }
        public int CheckCardUsing(int cardId)
        {
            int CheckCardReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "select * from CheckInOut where card_id = '"+cardId+"' and checkout_time is null;";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                if(reader.Read()){
                    CheckCardReturn = EXIST;
                } else {
                    CheckCardReturn = 0; // 0 is not exist
                }
                reader.Close();
                connection.Close();
            }catch{
                CheckCardReturn = ERROR;
            }
            return CheckCardReturn;
        }
        public int CheckSaveCheckIn(string licensePlate)
        {
            int CheckSaveReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "select * from CheckInOut where vehicle_number = '"+licensePlate+"' and checkout_time is null;";
                MySqlDataReader reader = DbHelper.ExecQuery(query);
                if(reader.Read()){
                    CheckSaveReturn = EXIST;
                } else {
                    CheckSaveReturn = 0; // 0 is not exist
                }
                reader.Close();
                connection.Close();
            }catch{
                CheckSaveReturn = ERROR;
            }
            return CheckSaveReturn;
        }
        public int SaveCheckIn(int cardId, int UserId, string licensePlate)
        {
            int SaveReturn;
            try{
                MySqlConnection connection = DbHelper.OpenConnection();
                string query = "insert into CheckInOut(card_id, checkin_time, checkin_user_id, vehicle_number) values ('"+
                                cardId+"',CURRENT_TIMESTAMP(),'"+UserId+"','"+licensePlate+"');";
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
