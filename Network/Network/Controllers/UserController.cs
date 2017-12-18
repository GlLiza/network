﻿using Microsoft.AspNet.Identity;
using Network.BL.WebServices;
using Network.DAL.EFModel;
using Network.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Network.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

      
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET: User
        [Authorize]        
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            UserIndexViewModel model = new UserIndexViewModel();
            model.AspUserId = id;           

            if (!User.IsInRole("secretary"))
            {
                var user = _userService.GetUserByAspNetId(id);
                model.Id = user.Id;

                var data = _userService.GetDataByAspUserId(id);
                if (data != null)
                {
                    model.Name = data.Name;
                    model.Image = _userService.GetImageByDataId(data.Id);

                }

                var contact = _userService.GetContactById(data.Id);
                if (contact != null)
                {
                    model.Skype = contact.Skype;
                    model.PhoneNumber = contact.PhoneNumber;
                }
              

                var aducation = _userService.GetAducationInf(model.Id);
                if (aducation != null)
                {
                    model.Type = aducation.Type;
                    model.Institution = aducation.Institution;
                    model.Specialization = aducation.Specialization;
                    model.StartYear = aducation.StartYear;
                    model.GradYear = aducation.GradYear;
                }          

                return View(model);
            }
            return View(model);         
        }




        public ActionResult AddUser(string id)
        {
            AddUserViewModel model = new AddUserViewModel()
            {
                AspUserId = id
            };
            return View("AddUser", model);
        }

        [HttpPost]
        public ActionResult AddUser(AddUserViewModel model)
        {
            if (model != null)
            {
                Image image = new Image()
                {
                    Image1 = _userService.СonvertingImg(model.Image)
                };
                _userService.AddImage(image);


                User_sPersonalData data = new User_sPersonalData()
                {
                    Name = model.Name,
                    ImageId = image.Id
                };

                _userService.AddPersData(data);

                User_sContact contact = new User_sContact()
                {
                    PhoneNumber = model.PhoneNumber,
                    Skype = model.Skype
                };
                _userService.AddContact(contact);

                User user = new User()
                {
                    PersonalDataId = data.Id,
                    ContactId = contact.Id,
                    Visibility = true,
                    AspUserId = model.AspUserId
                };
                _userService.AddUser(user);
            }
            return View("DisplayEmail");
        }


        public ActionResult BrowseUser()
        {
                List<UserListViewModel> model = new List<UserListViewModel>();
                var listIdsUsers = _userService.GetAllUsersId();

                var data = _userService.GetDataForListOfUserByAspId(listIdsUsers);

                foreach (var item in data)
                {
                    UserListViewModel user = new UserListViewModel();
                    user.Id = item.Id;
                    user.Name = item.Name;
                    user.Image = _userService.GetImageByDataId(item.Id);

                    model.Add(user);
                }

            return View();
        }


        public ActionResult GetAllUsers()
        {
            List<UserListViewModel> model = new List<UserListViewModel>();
            var listIdsUsers = _userService.GetAllUsersId();

            var data = _userService.GetDataForListOfUserByAspId(listIdsUsers);

            foreach (var item in data)
            {
                UserListViewModel user = new UserListViewModel();
                user.Id = item.Id;
                user.Name = item.Name;
                user.Image = _userService.GetImageByDataId(item.Id);

                model.Add(user);
            }
            return PartialView(model);
        }

        public ActionResult GetLead()
        {
            List<UserListViewModel> model = new List<UserListViewModel>();
            var listIdsUsers = _userService.GetAllLeadListId();

            var data = _userService.GetDataForListOfUserByAspId(listIdsUsers);
            foreach (var item in data)
            {
                UserListViewModel user = new UserListViewModel();
                user.Id = item.Id;
                user.Name = item.Name;
                user.Image = _userService.GetImageByDataId(item.Id);

                model.Add(user);
            }
            return PartialView(model);
        }

        public ActionResult GetMemberGroup()
        {
            List<UserListViewModel> model = new List<UserListViewModel>();
            var listIdsUsers = _userService.GetAllMemberListId();
            var data = _userService.GetDataForListOfUserByAspId(listIdsUsers);
            foreach (var item in data)
            {
                UserListViewModel user = new UserListViewModel();
                user.Id = item.Id;
                user.Name = item.Name;
                user.Image = _userService.GetImageByDataId(item.Id);

                model.Add(user);
            }

            return PartialView(model);
        }

        public ActionResult GetContact(Guid idUser)
        {
            var user = _userService.GetUserById(idUser);
            var contact = _userService.GetContactById(user.ContactId);
            return PartialView("_ShowContact",contact);
        }

    }
}
