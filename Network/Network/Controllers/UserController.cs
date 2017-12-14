﻿using Microsoft.AspNet.Identity;
using Network.BL.WebServices;
using Network.DAL.EFModel;
using Network.Views.ViewModels;
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
            //var id = User.Identity.GetUserId();
            //UserListIndexViewModel model = new UserListIndexViewModel();
            //model.AspUserId = id;
            //var user = _userService.GetUserByAspNetId(id);
            //model.Id = user.Id;

            //var data = _userService.GetDataByAspUserId(id);
            //model.Name = data.Name;
            //model.Image = _userService.GetImageByDataId(data.Id);
            

            return View();
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
                    Email = User.Identity.Name,
                    PhoneNumber = model.PhoneNumber,
                    Skype = model.Skype
                };
                _userService.AddContact(contact);

                User user = new User()
                {
                    PersonalDataId = data.Id,
                    ContactId = contact.Id,
                    Visibility = true,
                   // Type = Convert.ToString(model.TypeUser),
                    AspUserId = model.AspUserId
                };
               // UserManager//AddToRole(model.AspUserId, model.TypeUser.ToString());


                _userService.AddUser(user);
            }
            return View("DisplayEmail");
        }


        public ActionResult BrowseUser()
        {
            List<UserListViewModel> model = new List<UserListViewModel>();
            var listIdsUsers = _userService.GetUsersIdWithoutCurrent(User.Identity.GetUserId());

            var data = _userService.GetDataForListOfUserByAspId(listIdsUsers);

            foreach (var item in data)
            {
                UserListViewModel user = new UserListViewModel();
                user.Id = item.Id;
                user.Name = item.Name;
                user.Image = _userService.GetImageByDataId(item.Id);

                model.Add(user);
            }


            return View(model);
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


            return View("BrowseUser",model);
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

            return View("BrowseUser", model);
        }

        //public List<UserListViewModel> AllUser()
        //{
        //    return null;
        //}




    }
}
