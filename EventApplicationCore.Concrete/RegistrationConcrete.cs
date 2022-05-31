﻿using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace EventApplicationCore.Concrete
{
    public class RegistrationConcrete : IRegistration
    {
        private DatabaseContext _context;

        public RegistrationConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public void AddAdmin(Registration entity)
        {
            _context.Registration.Add(entity);
            _context.SaveChanges();
        }

        public int AddUser(Registration entity)
        {
            _context.Registration.Add(entity);
            return _context.SaveChanges();
        }

        public bool CheckUserNameExists(string Username)
        {
            var result = (from user in _context.Registration
                          where user.Username == Username
                          select user).Count();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateUserProfile (RegistrationViewModel model)
        {
           var user= _context.Registration.Where(x=>x.ID==model.ID).FirstOrDefault();
            user.Name = model.Name;
            user.Mobileno = model.Mobileno;
            user.Address = model.Address;
            user.Birthdate = DateTime.Parse(model.Birthdate, new System.Globalization.CultureInfo("en-GB"));
            user.Username = model.Username;
            user.EmailID = model.EmailID;
            user.Area = model.Area;
              _context.Registration.Update(user);
            _context.SaveChanges();
        }
        public RegistrationViewModel Userinformation(int UserID)
        {
            var result = (from user in _context.Registration
                          join country in _context.Country on user.Country equals country.CountryID
                          join states in _context.States on user.State equals states.StateID
                          join city in _context.City on user.City equals city.CityID
                          where user.ID == UserID
                          select new RegistrationViewModel
                          
                          { ID=user.ID,
                              CountryName = country.Name ,
                            StateName = states.StateName,
                            CityName = city.CityName,
                            Name =user.Name,
                            Address = user.Address,
                            EmailID =user.EmailID,
                            CreatedOn = user.CreatedOn.Value.ToString("dd/MM/yyyy"),
                            Birthdate = user.Birthdate.Value.ToString("dd/MM/yyyy"),
                            Gender = user.Gender == "M" ? "Male":"Female",
                            Mobileno = user.Mobileno,
                            Username = user.Username,
                            Password = user.Password,
                            City=user.City,
                            Country=user.Country,
                            State=user.State,
                            Area=user.Area
                          }).SingleOrDefault();
            return result;
        }

        public IQueryable<RegistrationViewModel> UserinformationList(string sortColumn, string sortColumnDir, string Search)
        {
            var IQueryableReg = (from user in _context.Registration
                                 join country in _context.Country on user.Country equals country.CountryID
                                 join states in _context.States on user.State equals states.StateID
                                 join city in _context.City on user.City equals city.CityID
                                 select new RegistrationViewModel
                                 {
                                     CountryName = country.Name,
                                     StateName = states.StateName,
                                     CityName = city.CityName,
                                     Name = user.Name,
                                     Address = user.Address,
                                     EmailID = user.EmailID,
                                     CreatedOn = user.CreatedOn.Value.ToString("dd/MM/yyyy"),
                                     Birthdate = user.Birthdate.Value.ToString("dd/MM/yyyy"),
                                     Gender = user.Gender == "M" ? "Male" : "Female",
                                     Mobileno = user.Mobileno,
                                     Username = user.Username
                                 });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableReg = IQueryableReg.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableReg = IQueryableReg.Where(m => m.Username == Search || m.Mobileno == Search || m.EmailID == Search);
            }

            return IQueryableReg;
        }
    }
}
