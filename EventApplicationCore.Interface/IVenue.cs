using EventApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Interface
{
    public interface IVenue
    {
        void SaveVenue(Venue venue);
        void UpdateVenue(Venue venue);
        IQueryable<Venue> ShowVenue(string sortColumn, string sortColumnDir, string Search);
        IEnumerable<Venue> ShowAllVenue(int ?id);
        IEnumerable<Venue> ShowAllVenue();
        IEnumerable<Venue> ShowAllByUser(int ? id);
        int DeleteVenue(int id);
        Venue VenueByID(int id);
        bool CheckVenueNameAlready(string venueName);
    }
}
