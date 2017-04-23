using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
  #region AutoSearch

        
       
        [Serializable()]
        public class DestinationAutoCompleteSearchCity
        {
            public DestinationAutoCompleteSearchCity(string ID, string CityName)
            {
                this.ID = ID;
                this.CityName = CityName;
            }
            public string ID { get; set; }
            public string CityName { get; set; }
        }

      

        public enum RegionType1
        {
            None = 0,
            ADM1 = 1,
            ADM2 = 2,
            RGN = 3,
            AIRP = 4,
            PPLA = 5,
            PPLA2 = 6,
            PPLA3 = 7,
            PPLC = 8,
            PPL = 9,
            PPLX = 10,
            POI = 11,
        }

        public enum DestinationSearchType1
        {
            Region = 1,
            Hotel = 2,
        }

        public class DestinationAutoSearchWrapper
        {
            public List<DestinationAutoCompleteSearch> items { get; set; }
        }

        [Serializable()]
        public class DestinationAutoCompleteSearch
        {

            public string ID;

            public string DestinationSearchType;

            public string Name;

            public string CountryID;

            public string CountryName;

            public string Msgtopdest;

            public string RegionID;

            public string RegionName;

            public string ParentID;

            public string ParentName;

            public string SecondParentName;

            public string DisplayName;

            public string RegionType;

            public string Code;

            public Boolean IsPopular;

            public string HotelID;

            public string HotelName;

            public string DestinationSearchTypeImagePath;

            public const string ImagePath = "~/Images/";

            public DestinationAutoCompleteSearch(string ID, string DestinationSearchType, string CountryID, string CountryName, string RegionID, string RegionName, string ParentID, string ParentName, string SecondParentName, string RegionType, string Code, string HotelID, string HotelName, string IsPopular)
            {
                this.ID = ID;
                this.DestinationSearchType = DestinationSearchType;
                this.CountryID = CountryID;
                this.CountryName = CountryName;
                this.RegionID = RegionID;
                this.RegionName = RegionName;
                this.ParentID = ParentID;
                this.ParentName = ParentName;
                this.SecondParentName = SecondParentName;
                this.RegionType = RegionType;
                this.Code = Code;
                this.HotelID = HotelID;
                this.HotelName = HotelName;
                this.IsPopular = Convert.ToBoolean(IsPopular);

                //bool IsPopular = Convert.ToBoolean(this.IsPopular);

                //DestinationSearchType1 Regionenum = DestinationSearchType1.Region;
                int Regionvalue = (int)DestinationSearchType1.Region;
                string Region = Convert.ToString(Regionvalue);

                //DestinationSearchType1 Hotelenum = DestinationSearchType1.Hotel;
                int Hotelvalue = (int)DestinationSearchType1.Hotel;
                string Hotel = Convert.ToString(Hotelvalue);

                //RegionType1 AIRPenum = RegionType1.AIRP;
                int AIRPvalue = (int)RegionType1.AIRP;
                string AIRP = Convert.ToString(AIRPvalue);

                if ((DestinationSearchType == Region))
                {
                    this.Name = RegionName;
                    this.DisplayName = RegionName;
                }


                else if ((DestinationSearchType == Hotel))
                {
                    this.Name = (HotelName + (", " + RegionName));
                    this.DisplayName = this.Name;
                }

                if ((this.RegionType == AIRP && this.Code != String.Empty))
                {

                    this.DisplayName += (" (" + (this.Code + ")"));
                    this.RegionName += (" (" + (this.Code + ")"));
                    this.Name = this.RegionName;
                }


                if ((this.ParentName != string.Empty))
                {
                    this.DisplayName += (", " + this.ParentName);
                }

                if ((this.SecondParentName != String.Empty))
                {
                    this.DisplayName += (", " + this.SecondParentName);
                }

                this.DisplayName += (", " + this.CountryName);

                if ((DestinationSearchType == Region))
                {
                    if ((this.RegionType == AIRP))
                    {
                        this.DestinationSearchTypeImagePath = System.Web.VirtualPathUtility.ToAbsolute((ImagePath + "AirportIcon_ds.png"));
                    }
                    else if (this.IsPopular)
                    {
                        this.DestinationSearchTypeImagePath = System.Web.VirtualPathUtility.ToAbsolute((ImagePath + "PopularIcon_ds.png"));
                    }
                    else
                    {
                        this.DestinationSearchTypeImagePath = System.Web.VirtualPathUtility.ToAbsolute((ImagePath + "RegionIcon_ds.png"));
                    }
                }
                else if ((DestinationSearchType == Hotel))
                {
                    this.DestinationSearchTypeImagePath = System.Web.VirtualPathUtility.ToAbsolute((ImagePath + "HotelIcon_ds.png"));
                }
            }
        }

        #endregion AutoSearch
}