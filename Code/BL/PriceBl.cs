using System;
using DAL;
using System.Collections.Generic;
using Persistence;

namespace BL
{
    public class PriceBl
    {
        private PriceDal dal = new PriceDal();
        public bool checkManyDays(int inOutId){
            return dal.checkManyDays(inOutId) > 0; //>0 is many day
        }
        public int getHoursInDay(int inOutId){
            return dal.getHoursInDay(inOutId);
        }
        public int getHoursDayTime(int inOutId)
        {
            return dal.getHoursDayTime(inOutId);
        }

        public int getHoursInNight(int inOutId)
        {
            return dal.getHoursInNight(inOutId);
        }
        public int getHoursNightTime(int inOutId)
        {
            return dal.getHoursNightTime(inOutId);
        }
        public int getNight(int inOutId)
        {
            return dal.getNight(inOutId);
        }
        public int getDays(int inOutId){
            return dal.getDays(inOutId);
        }
        public int getDayPrice(int hDay)
        {
            return dal.getDayPrice(hDay);
        }
        public int getNightPrice(int hNight)
        {
            return dal.getNightPrice(hNight);
        }
        public int getOverNightPrice(int Night)
        {
            return dal.getOverNightPrice(Night);
        }
        public int getFullDayPrice(int day){
            return dal.getFullDayPrice(day);
        }
        public string getCheckinTime(int inOutId){
            return dal.getCheckinTime(inOutId);
        }
        public string getCheckoutTime(int inOutId){
            return dal.getCheckoutTime(inOutId);
        }
        public string getLicensePlate(int inOutId){
            return dal.getLicensePlate(inOutId);
        }
    }
}