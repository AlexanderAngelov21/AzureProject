﻿

namespace AleksandarWebDevelopment2Project.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Authorization;
    using Models;
    using Models.Cars;
    using Models.Posts;
    using Services.Cars;
    using Services.Posts;
    using Infrastructure;

    public class PostsController : Controller
    {
        private readonly ICarsService carsService;
        private readonly IPostsService postsService;
        private readonly IWebHostEnvironment environment;

        public PostsController(ICarsService carsService, IPostsService postsService, IWebHostEnvironment environment)
        {
            this.carsService = carsService;
            this.postsService = postsService;
            this.environment = environment;
        }

        [Authorize]
        public IActionResult Create()
        {
            var createPostInputModel = new PostFormInputModel();
            var createCarInputModel = new CarFormInputModel();

            this.carsService.FillBaseInputCarProperties(createCarInputModel);

            createPostInputModel.Car = createCarInputModel;

            return this.View(createPostInputModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostFormInputModel input)
        {
            var inputCar = input.Car;

            if (!this.ModelState.IsValid)
            {
                this.carsService.FillBaseInputCarProperties(inputCar);
                return this.View(input);
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var selectedExtrasIds = input.SelectedExtrasIds.ToList();
            var imagePath = $"{this.environment.WebRootPath}/images";
            int postId;

            try
            {
                var car = await this.carsService.GetCarFromInputModelAsync(inputCar, selectedExtrasIds, userId, imagePath);
                postId = await this.postsService.CreateAsync(input, car, userId);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("CustomError", ex.Message);
                this.carsService.FillBaseInputCarProperties(inputCar);
                return this.View(input);
            }

            TempData[GlobalConstants.GlobalSuccessMessageKey] = "Your car post was added successfully!";

            return this.RedirectToAction("Offer", new { Id = postId });
        }

        public IActionResult Search()
        {
            var searchPostInputModel = new SearchPostInputModel();
            var searchCarInputModel = new SearchCarInputModel();

            this.carsService.FillBaseInputCarProperties(searchCarInputModel);

            searchPostInputModel.Car = searchCarInputModel;

            return this.View(searchPostInputModel);
        }

        public IActionResult All(SearchPostInputModel searchPostInputModel, int id = 1, int sorting = 0)
        {
            try
            {
                if (id <= 0)
                {
                    return this.NotFound();
                }

                const int PostsPerPage = 12;

                var matchingPosts = this.postsService.GetMatchingPosts(searchPostInputModel, (PostsSorting)sorting).ToList();

                var postsListViewModel = new PostsListViewModel()
                {
                    PageNumber = id,
                    PostsPerPage = PostsPerPage,
                    PostsCount = matchingPosts.Count(),
                    Posts = this.postsService.GetPostsByPage(matchingPosts, id, PostsPerPage),
                };

                if (id > postsListViewModel.PagesCount)
                {
                    return this.NotFound();
                }

                return this.View(postsListViewModel);
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.GlobalErrorMessageKey] = ex.Message;
                this.carsService.FillBaseInputCarProperties(searchPostInputModel.Car);
                return this.View("Search", searchPostInputModel);
            }
        }

        public IActionResult Offer(int id)
        {
            var singlePostData = this.postsService.GetSinglePostViewModelById(id);

            return this.View(singlePostData);
        }

        [Authorize]
        public IActionResult Mine(int id = 1, int sorting = 0)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            const int PostsPerPage = 12;

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUserPosts = this.postsService.GetPostsByUser(userId, (PostsSorting)sorting).ToList();

            var postsByUserViewModel = new PostsByUserViewModel()
            {
                PageNumber = id,
                PostsPerPage = PostsPerPage,
                PostsCount = currentUserPosts.Count(),
                Posts = this.postsService.GetPostsByPage(currentUserPosts, id, PostsPerPage),
            };

            if (id > postsByUserViewModel.PagesCount && id > 1)
            {
                return this.NotFound();
            }

            return this.View(postsByUserViewModel);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var post = this.postsService.GetPostFormInputModelById(id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.CreatorId != userId)
            {
                return Unauthorized();
            }

            this.carsService.FillBaseInputCarProperties(post.Car);

            return this.View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, EditPostViewModel editedPost)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var editedCar = editedPost.Car;

            if (editedPost.CreatorId != userId)
            {
                return Unauthorized();
            }

            if (!this.ModelState.IsValid)
            {
                this.carsService.FillBaseInputCarProperties(editedCar);
                editedPost.CurrentImages = this.postsService.GetCurrentDbImagesForAPost(id);
                return this.View(editedPost);
            }

            var selectedExtrasIds = editedPost.SelectedExtrasIds.ToList();
            var deletedImagesIds = editedPost.DeletedImagesIds.ToList();
            var imagePath = $"{this.environment.WebRootPath}/images";

            try
            {
                await this.carsService.UpdateCarDataFromInputModelAsync(editedPost.CarId, editedCar, selectedExtrasIds, deletedImagesIds, userId, imagePath, editedPost.SelectedCoverImageId);
                await this.postsService.UpdateAsync(id, editedPost);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("CustomError", ex.Message);
                this.carsService.FillBaseInputCarProperties(editedCar);
                editedPost.CurrentImages = this.postsService.GetCurrentDbImagesForAPost(id);
                return this.View(editedPost);
            }

            TempData[GlobalConstants.GlobalSuccessMessageKey] = "Your car post was edited successfully!";

            return this.RedirectToAction("Offer", new { Id = id });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var post = this.postsService.GetBasicPostInformationById(id);

            if (post == null)
            {
                return NotFound();
            }

            var postCreatorId = this.postsService.GetPostCreatorId(id);

            if (userId != postCreatorId)
            {
                return Unauthorized();
            }

            return this.View(post);
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var postCreatorId = this.postsService.GetPostCreatorId(id);

            if (userId != postCreatorId)
            {
                return Unauthorized();
            }

            try
            {
                await this.postsService.DeletePostByIdAsync(id);
                TempData[GlobalConstants.GlobalSuccessMessageKey] = "Your car post was deleted successfully!";
            }
            catch (Exception ex)
            {
                this.TempData[GlobalConstants.GlobalErrorMessageKey] = ex.Message;
            }

            return this.RedirectToAction("Mine");
        }
    }
}