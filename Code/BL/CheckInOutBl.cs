using System;
using DAL;

namespace BL
{
    public class CheckInOutBl
    {
        private CheckinDal dal = new CheckinDal();
        private CheckOutDal outDal = new CheckOutDal();
        public bool CheckCardID(int id){
            return dal.CheckCardID(id) > 0;
        }
        public bool CheckLicensePlate(int id, string licensePlate){
            return dal.CheckLicensePlate(id, licensePlate) > 0;
        }
        public bool CheckDailyCard(int id){
            return dal.CheckDailyCard(id) > 0;
        }
        public bool SaveLicensePlate(int id, string licensePlate){
            return dal.SaveLicensePlate(id, licensePlate) > 0;
        }
        public bool CheckExpiryDate(int id){
            return dal.CheckExpiryDate(id) > 0;
        }
        public bool CheckSaveCheckIn(string licensePlate){
            return dal.CheckSaveCheckIn(licensePlate) > 0;
        }
        public bool CheckCardUsing(int cardId){
            return dal.CheckCardUsing(cardId) > 0;
        }
        public bool SaveCheckIn(int cardId, int UserId ,string licensePlate){
            return dal.SaveCheckIn(cardId, UserId, licensePlate) > 0;
        }
        public int CheckOutLicensePlate(int id, string licensePlate){
            return outDal.CheckOutLicensePlate(id, licensePlate);
        }
        public bool SaveCheckOutTime(int cardId, int UserId ){
            return outDal.SaveCheckOutTime(cardId, UserId) > 0;
        }
        public bool SaveCheckOut(int inOutId, int price ){
            return outDal.SaveCheckOut(inOutId, price) > 0;
        }
    }
    
}