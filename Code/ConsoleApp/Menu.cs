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
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            PriceDal test = new PriceDal();
            int UserId = -1, Revenue = 0, CheckinNumber = 0, checkoutNumber = 0;
            string strID = null, licensePlate = null, strRevenue = null;
            int cardId = -1;
            CheckInOutBl bl = new CheckInOutBl();
            LoginConsole loginU = new LoginConsole();
            PriceBl pBl = new PriceBl();
            do
            {
                Console.Clear();
                Heading();
                Console.WriteLine();
                Console.WriteLine(" ================================================================");
                Console.WriteLine("                          Login");
                Console.WriteLine(" ================================================================");
                UserId = loginU.CheckLogin();
                if (UserId < 0) Console.WriteLine("      Please try again!");
                Console.ReadKey();
            } while (UserId < 0);
            string choose = null;
            while (choose != "4")
            {
                
                Heading();
                Console.WriteLine("\n +--------------------------------------------------------------+");
                Console.WriteLine(" |                     CAR PARKING MANAGEMENT                   |");
                Console.WriteLine(" +--------------------------------------------------------------+");
                Console.WriteLine(" |         1.  Check In                                         |");
                Console.WriteLine(" |         2.  Check Out                                        |");
                Console.WriteLine(" |         3.  Show Revenue                                     |");
                Console.WriteLine(" |         4.  Logout                                           |");
                Console.WriteLine(" +--------------------------------------------------------------+");
                Console.Write("         Enter your choose: ");
                ConsoleKeyInfo keyInfo;
                choose = "";
                do
                {
                    keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.KeyChar >= '1' && keyInfo.KeyChar <= '4' && choose.Length == 0)
                    {
                        Console.Write(keyInfo.KeyChar);
                        choose = "";
                        choose += keyInfo.KeyChar;
                    }
                    else if (keyInfo.KeyChar >= '1' && keyInfo.KeyChar <= '4' && choose.Length == 1)
                    {
                        Console.Write("\b \b");
                        choose = choose[0..^1];
                        Console.Write(keyInfo.KeyChar);
                        choose = "";
                        choose += keyInfo.KeyChar;
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace && choose.Length > 0)
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
                        do
                        {
                            Heading();
                            Checkin();
                            strID = checkIn.CheckCardID();
                            if (strID == null)
                            {
                                Console.WriteLine("     Please try again!");
                                Console.ReadKey();
                            }
                        } while (strID == null);
                        Int32.TryParse(strID, out cardId);
                        if (checkIn.CheckCardUsing(cardId))
                        {
                            Console.WriteLine("     This card is being used!");
                        }
                        else
                        {
                            if (bl.CheckDailyCard(cardId))
                            {
                                Console.WriteLine("     Daily Card");
                                licensePlate = checkIn.SaveLicensePlate(cardId);
                                checkIn.SaveCheckIn(cardId, UserId, licensePlate);
                                CheckinNumber++;
                            }
                            else
                            {
                                do
                                {
                                    Heading();
                                    Checkin();
                                    Console.WriteLine("     Card ID: " + strID);
                                    Console.WriteLine("     Monthly Card");
                                    licensePlate = checkIn.CheckLicensePlate(cardId);
                                    if (licensePlate == null)
                                    {
                                        Console.WriteLine("     Please try again!");
                                        Console.ReadKey();
                                    }
                                } while (licensePlate == null);
                                if (!bl.CheckExpiryDate(cardId))
                                {
                                    Console.WriteLine("     This card is out of date! Check-in fail!");
                                }
                                else
                                {
                                    if (checkIn.SaveCheckIn(cardId, UserId, licensePlate))
                                    {
                                        CheckinNumber++;
                                    }
                                }
                            }
                        }

                        Console.ReadKey();
                        break;
                    case "2":
                        CheckConsole checkOut = new CheckConsole();
                        strID = null;
                        int inOutId = -1, hDay = 0, hNight = 0, Night = 0, price = 0, fullday=0;
                        cardId = -1;
                        do
                        {
                            Heading();
                            Checkout();
                            strID = checkOut.CheckCardID();
                            if (strID == null)
                            {
                                Console.WriteLine("     Please try again!");
                                Console.ReadKey();
                            }
                        } while (strID == null);
                        Int32.TryParse(strID, out cardId);
                        if (!checkOut.CheckCardUsing(cardId))
                        {
                            Console.WriteLine("     This card is not being used!");
                        }
                        else
                        {
                            do
                            {
                                Heading();
                                Checkout();
                                Console.WriteLine("     Card ID: " + strID);
                                inOutId = checkOut.CheckOutLicensePlate(cardId);
                                if (inOutId <= 0)
                                {
                                    Console.WriteLine("     Wrong license plate!\n      Please try again!");
                                    Console.ReadKey();
                                }
                            } while (inOutId <= 0);
                            checkOut.SaveCheckOutTime(cardId, UserId);
                            Console.ReadKey();
                            if (pBl.checkManyDays(inOutId))
                            {
                                hDay = pBl.getHoursDayTime(inOutId);
                                hNight = pBl.getHoursNightTime(inOutId);
                            }
                            else
                            {
                                hDay = pBl.getHoursInDay(inOutId);
                                hNight = pBl.getHoursInNight(inOutId);
                                fullday = pBl.getDays(inOutId);
                            }
                            Night = pBl.getNight(inOutId);
                            price = pBl.getDayPrice(10) + pBl.getNightPrice(3) + pBl.getOverNightPrice(2) + pBl.getFullDayPrice(5);
                            Revenue += price;
                            checkoutNumber++;
                            checkOut.SaveCheckOut(inOutId, price);
                            Heading();
                            ShowBill(pBl.getCheckinTime(inOutId), pBl.getCheckoutTime(inOutId), inOutId, price, pBl.getLicensePlate(inOutId));
                            Console.WriteLine(hDay+" | "+hNight+" | "+fullday +" | "+Night + " | inoutid: "+inOutId);
                        }
                        Console.ReadKey();
                        break;
                    case "3":
                        Heading();
                        ShowRevenue();
                        strRevenue = null;
                        string temp = Revenue.ToString(), text = null;
                        int leng = temp.Length;
                        for (int j = leng - 1; j >= 0; j--)
                        {
                            if ((leng - j - 1) % 3 == 0 && j != leng - 1) text += ",";
                            text += temp[j];
                        }
                        for (int k = text.Length - 1; k >= 0; k--)
                        {
                            strRevenue += text[k];
                        }
                        Console.WriteLine(" |     Check-in       :   {0,36}  |", CheckinNumber);
                        Console.WriteLine(" |     Check-out      :   {0,36}  |", checkoutNumber);
                        Console.WriteLine(" |     Revenue (VND)  :   {0,33}VND  |", strRevenue);
                        Console.WriteLine(" +--------------------------------------------------------------+");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.WriteLine("\n       Logout and end shift. Do you want to continue?(Y/N)");
                        string yn = "";
                        Console.Write("       Your chose: ");
                        do
                        {
                            keyInfo = Console.ReadKey(intercept: true);
                            if ((keyInfo.KeyChar == 'Y' || keyInfo.KeyChar == 'N' || keyInfo.KeyChar == 'n' || keyInfo.KeyChar == 'y') && yn.Length == 0)
                            {
                                string s = null;
                                s += keyInfo.KeyChar;
                                yn += keyInfo.KeyChar;
                                yn = yn.ToUpper();
                                Console.Write(s.ToUpper());
                            }
                            else if ( yn == "Y" &&(keyInfo.KeyChar == 'N' || keyInfo.KeyChar == 'n') && yn.Length == 1)
                            {
                                Console.Write("\b \b");
                                yn = yn[0..^1];
                                string s = null;
                                s += keyInfo.KeyChar;
                                yn += keyInfo.KeyChar;
                                yn = yn.ToUpper();
                                Console.Write(s.ToUpper());
                            }
                            else if ( yn == "N" && (keyInfo.KeyChar == 'Y' || keyInfo.KeyChar == 'y') && yn.Length == 1)
                            {
                                Console.Write("\b \b");
                                yn = yn[0..^1];
                                string s = null;
                                s += keyInfo.KeyChar;
                                yn += keyInfo.KeyChar;
                                yn = yn.ToUpper();
                                Console.Write(s.ToUpper());
                            }
                            else if (keyInfo.Key == ConsoleKey.Backspace && yn.Length > 0)
                            {
                                Console.Write("\b \b");
                                yn = yn[0..^1];
                            }
                        } while (keyInfo.Key != ConsoleKey.Enter);
                        if (yn == "Y")
                        {
                            Console.WriteLine("\n       Exit application.");
                        }
                        else choose = "1";
                        break;
                    default:
                        Console.WriteLine("\n       Wrong Key. Try Again!");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static void Checkin()
        {
            Console.WriteLine();
            Console.WriteLine(" ================================================================");
            Console.WriteLine("                            Check In");
            Console.WriteLine(" ================================================================");
        }
        static void Checkout()
        {
            Console.WriteLine();
            Console.WriteLine(" ================================================================");
            Console.WriteLine("                           Check Out");
            Console.WriteLine(" ================================================================");
        }
        static void ShowRevenue()
        {
            Console.WriteLine(" ================================================================");
            Console.WriteLine("                   CAR PARKING MANAGEMENT ");
            Console.WriteLine(" +--------------------------------------------------------------+");
            Console.WriteLine(" |                       Show Revenue                           |");
            Console.WriteLine(" +--------------------------------------------------------------+");
        }
        static void ShowBill(string checkin, string checkout, int inOutId, int price, string licensePlate)
        {
            Console.WriteLine();
            Console.WriteLine(" +--------------------------------------------------------------+");
            Console.WriteLine(" |                         CAR PARKING                          |");
            Console.WriteLine(" +--------------------------------------------------------------+");
            Console.WriteLine(" |                          ID : {0}                         |", numb(inOutId, 6));
            Console.WriteLine(" |                   License Plate : {0,8}                   |", licensePlate);
            Console.WriteLine(" |    Check-in time      : {0,33}    |", checkin);
            Console.WriteLine(" |    Check-out time     : {0,33}    |", checkout);
            Console.WriteLine(" |    Total price (VND)  : {0,30}VND    |", price.ToString("0,000"));
            Console.WriteLine(" +--------------------------------------------------------------+");
        }
        static void Heading()
        {
            Console.Clear();
            Console.WriteLine(" ================================================================");
            Console.WriteLine("                   CAR PARKING MANAGEMENT              ");
            Console.WriteLine("             PF11 - Hoàng Gia Luân - Bùi Phương Dung");
            Console.WriteLine("                 Teacher: Nguyễn Xuân Sinh");
            Console.WriteLine(" ================================================================");
        }
        static string numb(int n, int len)
        {
            string str = n.ToString(), temp = "";
            int a = str.Length;
            for (int i = 0; i < len - a; i++)
            {
                temp += "0";
            }
            temp = temp + str;
            return temp;
        }
    }
}
