using System;
using BL;
using System.Collections.Generic; 
using Persistence;
using DAL;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            PriceDal test =  new PriceDal();
            int UserId = -1, Revenue = 0, CheckinNumber = 0, checkoutNumber = 0;
            string strID = null, licensePlate = null, strRevenue = null;
            int cardId = -1;
            CheckInOutBl bl = new CheckInOutBl();
            LoginConsole loginU = new LoginConsole();
            PriceBl pBl =  new PriceBl();
            do{
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("       CAR PARKING MANAGEMENT ");
                Console.WriteLine("=================================");
                Console.WriteLine("              Login");
                Console.WriteLine("=================================");
                UserId = loginU.CheckLogin();
                if(UserId < 0) Console.WriteLine("Please try again!");
                Console.ReadKey();
            }while(UserId < 0);
            string choose = null;
            while (choose != "4")
            {
                Console.Clear();
                Console.WriteLine("\n+----------------------------------+");
                Console.WriteLine("|      CAR PARKING MANAGEMENT      |");
                Console.WriteLine("+----------------------------------+");
                Console.WriteLine("|  1. Check In                     |");
                Console.WriteLine("|  2. Check Out                    |");
                Console.WriteLine("|  3. Show Revenue                 |");
                Console.WriteLine("|  4. Logout                       |");
                Console.WriteLine("+----------------------------------+");
                Console.Write("   Enter your choose: ");
                ConsoleKeyInfo keyInfo;
                choose = null;
                do
                {
                    keyInfo = Console.ReadKey(intercept: true);
                    if (char.IsNumber(keyInfo.KeyChar))
                    {
                        Console.Write(keyInfo.KeyChar);
                        choose += keyInfo.KeyChar;
                    }else if (keyInfo.Key == ConsoleKey.Backspace && choose.Length > 0)
                    {
                        Console.Write("\b \b");
                        choose = choose[0..^1];
                    }
                } while (keyInfo.Key != ConsoleKey.Enter);
                switch (choose)
                {
                case "1":
                    CheckConsole checkIn = new CheckConsole();
                    strID = null;
                    licensePlate = null;
                    cardId = -1;
                    do{
                        Checkin();
                        strID = checkIn.CheckCardID();
                        if(strID == null)
                        {
                            Console.WriteLine("Please try again!");
                            Console.ReadKey();
                        }
                    }while(strID == null);
                    Int32.TryParse(strID, out cardId);
                    if(checkIn.CheckCardUsing(cardId)){
                        Console.WriteLine("This card is being used!");
                    }else{
                        if(bl.CheckDailyCard(cardId))
                        {
                            licensePlate = checkIn.SaveLicensePlate(cardId);
                            checkIn.SaveCheckIn(cardId, UserId, licensePlate);
                            CheckinNumber ++;
                        }else{
                            do{
                                Checkin();
                                Console.WriteLine("Card ID: "+strID);
                                licensePlate = checkIn.CheckLicensePlate(cardId);
                                if(licensePlate == null){
                                    Console.WriteLine("Please try again!");
                                    Console.ReadKey();
                                }
                            }while(licensePlate == null);
                            if(!bl.CheckExpiryDate(cardId)){
                                Console.WriteLine("This card is out of date! Check-in fail!");
                            }
                            else{
                                checkIn.SaveCheckIn(cardId, UserId, licensePlate);
                                CheckinNumber++;
                            }
                        }
                    }
                    
                    Console.ReadKey();
                    break;
                case "2":
                    CheckConsole checkOut = new CheckConsole();
                    strID = null;
                    int inOutId = -1, hDay=0, hNight=0, Night=0, price = 0;
                    cardId = -1;
                    do{
                        Checkout();
                        strID = checkOut.CheckCardID();
                        if(strID == null)
                        {
                            Console.WriteLine("Please try again!");
                            Console.ReadKey();
                        }
                    }while(strID == null);
                    Int32.TryParse(strID, out cardId);
                    if(!checkOut.CheckCardUsing(cardId)){
                        Console.WriteLine("This card is not being used!");
                    }else{
                        do{
                            Checkout();
                            Console.WriteLine("Card ID: "+strID);
                            inOutId = checkOut.CheckOutLicensePlate(cardId);
                            if(inOutId <= 0){
                                Console.WriteLine("Wrong license plate!\nPlease try again!");
                                Console.ReadKey();
                            }
                        }while(inOutId <= 0);
                        checkOut.SaveCheckOutTime(cardId, UserId);
                        Console.ReadKey();
                        if(pBl.checkManyDays(inOutId)){
                            hDay = pBl.getHoursDayTime(inOutId);
                            hNight = pBl.getHoursNightTime(inOutId);                                
                        }else{
                            hDay = pBl.getHoursInDay(inOutId);
                            hNight = pBl.getHoursInNight(inOutId);
                        }
                        Night = pBl.getNight(inOutId);
                        price = pBl.getDayPrice(hDay) + pBl.getNightPrice(hNight) + pBl.getOverNightPrice(Night);
                        Revenue += price;
                        checkoutNumber++;
                        checkOut.SaveCheckOut(inOutId,price);
                        ShowBill(pBl.getCheckinTime(inOutId), pBl.getCheckoutTime(inOutId),inOutId, price, pBl.getLicensePlate(inOutId));
                    }
                    Console.ReadKey();
                    break;
                case "3":
                        ShowRevenue();
                        strRevenue = null;
                        string temp = Revenue.ToString(), text = null;
                        int leng = temp.Length;
                        for(int j = leng-1; j>=0; j--){
                            if((leng - j-1)%3 == 0 && j != leng-1) text += ",";
                            text += temp[j];
                        }
                        for(int k = text.Length-1; k>=0; k--){
                            strRevenue+=text[k];
                        }
                        Console.WriteLine("|     Check-in     : {0,17}  |",CheckinNumber);
                        Console.WriteLine("|     Check-out    : {0,17}  |",checkoutNumber);
                        Console.WriteLine("|     Revenue(VND) : {0,17}  |",strRevenue);
                        Console.WriteLine("+---------------------------------------+");
                    Console.ReadKey();
                    break;

                case "4":
                    Console.WriteLine("\nLogout and end shift. Do you want to continue?(Y/N)");
                    string yn = "";
                    do
                    {
                        keyInfo = Console.ReadKey(intercept: true);
                        if ((keyInfo.KeyChar == 'Y' ||keyInfo.KeyChar == 'N' ||keyInfo.KeyChar == 'n' ||keyInfo.KeyChar == 'y') && yn.Length == 0)
                        {
                                string s = null;
                                s+=keyInfo.KeyChar;
                                yn += keyInfo.KeyChar;
                                yn = yn.ToUpper();
                                Console.Write(s.ToUpper());
                        }else if (keyInfo.Key == ConsoleKey.Backspace && yn.Length > 0)
                        {
                            Console.Write("\b \b");
                            yn = yn[0..^1];
                        }
                    } while (keyInfo.Key != ConsoleKey.Enter);
                    if(yn == "Y"){
                        Console.WriteLine("\nExit application.");
                    }
                    else choose = "1";
                    break;
                default:
                    Console.WriteLine("\nWrong Key. Try Again!");
                    Console.ReadKey();
                    break;
                }
            }
        }
        static void Checkin()
        {
            Console.Clear();
            Console.WriteLine("============================================");
            Console.WriteLine("          CAR PARKING MANAGEMENT ");
            Console.WriteLine("============================================");
            Console.WriteLine("                 Check In");
            Console.WriteLine("============================================");
        }
        static void Checkout()
        {
            Console.Clear();
            Console.WriteLine("============================================");
            Console.WriteLine("          CAR PARKING MANAGEMENT ");
            Console.WriteLine("============================================");
            Console.WriteLine("                 Check Out");
            Console.WriteLine("============================================");
        }
        static void StartShift(){
            Console.Clear();
            Console.WriteLine("============================================");
            Console.WriteLine("          CAR PARKING MANAGEMENT ");
            Console.WriteLine("============================================");
            Console.WriteLine("                 Start Shift");
            Console.WriteLine("============================================");
        }
        static void EndShift(){
            Console.Clear();
            Console.WriteLine("============================================");
            Console.WriteLine("          CAR PARKING MANAGEMENT ");
            Console.WriteLine("============================================");
            Console.WriteLine("                 End Shift");
            Console.WriteLine("============================================");
        }
        static void ShowRevenue(){
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("          CAR PARKING MANAGEMENT ");
            Console.WriteLine("+---------------------------------------+");
            Console.WriteLine("|              Show Revenue             |");
            Console.WriteLine("+---------------------------------------+");
        }
        static void ShowBill(string checkin, string checkout, int inOutId, int price, string licensePlate)
        {
            Console.WriteLine("+---------------------------------------+");
            Console.WriteLine("|              CAR PARKING              |");
            Console.WriteLine("+---------------------------------------+");
            Console.WriteLine("|               ID : {0}             |",numb(inOutId, 6));
            Console.WriteLine("|        License Plate : {0,8}       |",licensePlate);
            Console.WriteLine("|Check-in time   : {0,21}|",checkin);
            Console.WriteLine("|Check-out time  : {0,21}|",checkout);
            Console.WriteLine("|Total price(VND): {0,21}|",price.ToString("0,000"));
            Console.WriteLine("+---------------------------------------+");
        }
        static string numb(int n, int len){
            string str = n.ToString(), temp = "";
            int a=str.Length;
            for (int i = 0; i < len-a; i++)
            {
                temp += "0";
            }
            temp = temp+str;
            return temp;
        }
    }
}
