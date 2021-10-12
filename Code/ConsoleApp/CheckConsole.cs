using System;
using BL;

namespace ConsoleApp
{
    public class CheckConsole
    {
        public string CheckCardID()
        {
            CheckInOutBl bl = new CheckInOutBl();
            Console.Write("Card ID: ");
            int cardId = -1;
            var strID = string.Empty;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(intercept: true);
                if (char.IsNumber(keyInfo.KeyChar))
                {
                    Console.Write(keyInfo.KeyChar);
                    strID += keyInfo.KeyChar;
                }else if (keyInfo.Key == ConsoleKey.Backspace && strID.Length > 0)
                    {
                        Console.Write("\b \b");
                        strID = strID[0..^1];
                    }
            } while (keyInfo.Key != ConsoleKey.Enter);
            Int32.TryParse(strID, out cardId);
            if(bl.CheckCardID(cardId)){
                Console.WriteLine();
                return strID;
            }else {
                Console.WriteLine("\nCard ID is not exist!");
                return null; //  Card ID is not exist
            }
        }
        public bool CheckCardUsing(int cardId)
        {
            CheckInOutBl bl = new CheckInOutBl();
            if(bl.CheckCardUsing(cardId)) return true;
            else return false;
        }

        public string CheckLicensePlate(int cardId)
        {
            CheckInOutBl bl = new CheckInOutBl();
            Console.Write("License plate: ");
            string licensePlate = Console.ReadLine().ToUpper();
            if(bl.CheckLicensePlate(cardId, licensePlate)){
                return licensePlate;
            }else {
                Console.WriteLine("Wrong license plate!");
                return null;
            }
        }
        public string SaveLicensePlate(int id)
        {
            CheckInOutBl bl =  new CheckInOutBl();
            Console.Write("License plate: ");
            string licensePlate = Console.ReadLine().ToUpper();
            bl.SaveLicensePlate(id, licensePlate);
            return licensePlate;
        }
        public void SaveCheckIn(int cardId, int UserId ,string licensePlate){
            CheckInOutBl bl =  new CheckInOutBl();
            if(bl.CheckSaveCheckIn(licensePlate)){
                Console.WriteLine("Check-in fail. This car has been check-in!");
            }
            else{
                bl.SaveCheckIn(cardId, UserId, licensePlate);
                Console.WriteLine("Successful check-in!");
            }
        }
        public int CheckOutLicensePlate(int cardId)
        {
            CheckInOutBl bl = new CheckInOutBl();
            Console.Write("License plate: ");
            string licensePlate = Console.ReadLine().ToUpper();
            int inOutId = bl.CheckOutLicensePlate(cardId, licensePlate);
            return inOutId;
        }

        public void SaveCheckOutTime(int cardId, int UserId){
            CheckInOutBl bl =  new CheckInOutBl();
            
            bl.SaveCheckOutTime(cardId, UserId);
            Console.WriteLine("Successful check-out!");
        }

        public void SaveCheckOut(int inOutId, int price){
            CheckInOutBl bl =  new CheckInOutBl();
            
            bl.SaveCheckOut(inOutId, price);
            Console.WriteLine("Successful check-out!");
        }
    }
}
