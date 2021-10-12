using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Persistence;

namespace DAL
{
    public class PriceDal
    {
        public static int ERROR = -1;
        public static int WRONG = 0;
        public static int EXIST = 1;
        private string query;
        private MySqlConnection connection = DbConfig.GetDefaultConnection();
        public int getHoursInDay(int inOutId)
        {
            int hour = 0;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select hour(timediff(time (checkin_time), time(checkout_time))) as hour from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='18:00' and time(checkin_time)>'06:00'and time(checkout_time)<='18:00' and time(checkout_time)>'06:00';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour = reader.GetInt32("hour")+1;
                }
                reader.Close();

                query = @"select hour(timediff(time (checkin_time), '18:00')) as hour from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='18:00' and time(checkin_time)>'06:00'and time(checkout_time)>'18:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour = reader.GetInt32("hour")+1;
                }
                reader.Close();

                query = @"select hour(timediff(time '06:00', time(checkout_time))) as hour from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='06:00' and time(checkout_time)<='18:00' and time(checkout_time)>'06:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour = reader.GetInt32("hour")+1;
                }
                reader.Close();

                query = @"select * from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='06:00' and time(checkout_time)>'18:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour = 12;
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return hour;
        }
        public int getHoursDayTime(int inOutId)
        {
            int hour = 0;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select hour(timediff(time (checkin_time), '18:00')) as hour from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='18:00' and time(checkin_time)>'06:00';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour += reader.GetInt32("hour");
                    hour++;
                }
                reader.Close();
                query = @"select hour(timediff(time (checkout_time), '6:00')) as hour from CheckInOut where inout_id = '"+inOutId+"' and time(checkout_time)>'06:00' and time(checkout_time)<='18:00';";
                command.CommandText = query;
                reader= command.ExecuteReader();
                if(reader.Read())
                {
                    hour += reader.GetInt32("hour");
                    hour ++;
                }
                reader.Close();
                query = @"select * from CheckInOut where inout_id = '"+inOutId+"' and time(checkout_time)>'18:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour += 12;
                }
                reader.Close();
                query = @"select * from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='06:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour += 12;
                }
                reader.Close();
                query = @"select datediff(checkout_time, checkin_time) as date from CheckInOut where inout_id = '"+inOutId+"';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour = hour + (reader.GetInt32("date")-1)*12;
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return hour;
        }
        public int checkManyDays(int inOutId){
            int CheckReturn = -1;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select datediff(checkout_time, checkin_time) as date from CheckInOut where inout_id = '"+inOutId+"';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    CheckReturn = reader.GetInt32("date");
                }else
                {
                    CheckReturn = 0;
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return CheckReturn;
        }

        public int getHoursInNight(int inOutId)
        {
            int hour = 0;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select hour(timediff(time (checkin_time), time(checkout_time))) as hour from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='23:00' and time(checkin_time)>'18:00'and time(checkout_time)<='23:00' and time(checkout_time)>'18:00';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour = reader.GetInt32("hour")+1;
                }
                reader.Close();

                query = @"select hour(timediff(time (checkin_time), '23:00')) as hour from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='23:00' and time(checkin_time)>'18:00'and time(checkout_time)>'23:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour = reader.GetInt32("hour")+1;
                }
                reader.Close();
                query = @"select hour(timediff(time '18:00', time(checkout_time))) as hour from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='18:00' and time(checkout_time)<='23:00' and time(checkout_time)>'18:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour = reader.GetInt32("hour")+1;
                }
                reader.Close();

                query = @"select * from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='18:00' and time(checkout_time)>'23:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour = 12;
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return hour;
        }
        public int getHoursNightTime(int inOutId)
        {
            int hour = 0;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select hour(timediff(time (checkin_time), '23:00')) as hour from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='23:00' and time(checkin_time)>'18:00';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour += reader.GetInt32("hour");
                    hour++;
                }
                reader.Close();
                query = @"select hour(timediff(time (checkout_time), '6:00')) as hour from CheckInOut where inout_id = '"+inOutId+"' and time(checkout_time)>'18:00' and time(checkout_time)<='23:00';";
                command.CommandText = query;
                reader= command.ExecuteReader();
                if(reader.Read())
                {
                    hour += reader.GetInt32("hour");
                    hour ++;
                }
                reader.Close();
                query = @"select * from CheckInOut where inout_id = '"+inOutId+"' and time(checkout_time)>'23:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour += 12;
                }
                reader.Close();
                query = @"select * from CheckInOut where inout_id = '"+inOutId+"' and time(checkin_time)<='18:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour += 12;
                }
                reader.Close();
                query = @"select datediff(checkout_time, checkin_time) as date from CheckInOut where inout_id = '"+inOutId+"';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    hour = hour + (reader.GetInt32("date")-1)*12;
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return hour;
        }
        public int getNight(int inOutId)
        {
            int Night = 0;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select datediff(ADDTIME(checkout_time, '01:00'), ADDTIME(checkin_time, '01:00')) as date from CheckInOut where inout_id = '"+inOutId+"';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    Night = reader.GetInt32("date");
                }
                reader.Close();

                query = @"select * from CheckInOut where inout_id = '"+inOutId+"' and time(ADDTIME(checkin_time, '01:00')) < '06:00';";
                command.CommandText = query;
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    Night ++;
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return Night;
        }
        public int getDayPrice(int hDay){
            int dPrice = 0;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select price from PriceTable where price_id = '1';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    dPrice = reader.GetInt32("price")*hDay;
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return dPrice;
        }

        public int getNightPrice(int hNight){
            int nPrice = 0;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select price from PriceTable where price_id = '2';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    nPrice = reader.GetInt32("price")*hNight;
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return nPrice;
        }

        public int getOverNightPrice(int Night){
            int overNightPrice = 0;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select price from PriceTable where price_id = '3';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    overNightPrice = reader.GetInt32("price")*Night;
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return overNightPrice;
        }
        public string getCheckinTime(int inOutId){
            string time = "";
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select checkin_time from CheckInOut where inout_id = '"+inOutId+"';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    time = reader.GetString("checkin_time");
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return time;
        }
        public string getCheckoutTime(int inOutId){
            string time = "";
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select checkout_time from CheckInOut where inout_id = '"+inOutId+"';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    time = reader.GetString("checkout_time");
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return time;
        }
        public string getLicensePlate(int inOutId){
            string licensePlate = "";
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"select vehicle_number from CheckInOut where inout_id = '"+inOutId+"';";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    licensePlate = reader.GetString("vehicle_number");
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return licensePlate;
        }
    }
}
