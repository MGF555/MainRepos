using CodeChampsAI.Data;
using CodeChampsAI.Data.Identity;
using CodeChampsAI.Models;
using CodeChampsAI.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChampsAI.Tests
{
	[TestFixture]
	public class MockTests
	{
        static UserManager<AppUser> userManager = new UserManager<AppUser>(new UserStore<AppUser>(new ApplicationDbContext()));
        private IRepository _repo = DIContainer.Kernel.Get<IRepository>();


        [Test]
		public void CanLoadPostByUserId()
		{
			string username = "AdamAdmin";
            var posts = _repo.GetPostsByUserName(username);

			Assert.AreEqual(5, posts.Count());

			username = "ConnieContributor";
            var morePosts = _repo.GetPostsByUserName(username);

			Assert.AreEqual(2, morePosts.Count());
		}

		[Test]
		public void CanSaveAndDeletePost()
		{
            int beforeCount = _repo.GetAllPosts().Count();
            string tagString = "NarrowAI";
            List<Tag> tags = _repo.TagStringToTags(tagString);

            Post newPost = new Post
            {
                ApprovalStatus = ApprovalEnums.Approved,
                IsFeatured = false,
                Date = DateTime.Parse("4/5/2016"),
                Subject = "End of the world",
                Body = "AI is the breatest threat to humanity",
                Tags = tags,
                User = userManager.FindByName("AdamAdmin")
			};

            _repo.SaveNewPost(newPost);

            int afterAddCount = _repo.GetAllPosts().Count();

            Assert.AreEqual(beforeCount + 1, afterAddCount);

            var postToDelete = _repo.GetPostById(newPost.PostId);
            _repo.DeletePost(postToDelete);


            var afterDeleteCount = _repo.GetAllPosts().Count();

            Assert.AreEqual(beforeCount, afterDeleteCount);
        }

        [Test]
		public void EditPost()
		{
			Post editedPost = new Post()
			{
				PostId = 2,
				ApprovalStatus = ApprovalEnums.Approved,
				IsFeatured = true,
				Date = DateTime.Parse("6/1/2015"),
				Subject = "Google AI Project GO!",
				Body = "Is beating the world best Go! player the greatest feet of man kind?",
				Tags = _repo.GetAllTags().Where(t => t.TagId == 1).ToList()
			};

			_repo.EditPost(editedPost);

            Post toCheck = _repo.GetPostById(2);

			Assert.AreEqual(2, toCheck.PostId);
            Assert.AreEqual(true, toCheck.IsFeatured);
            Assert.AreEqual("Google AI Project GO!", toCheck.Subject);
            Assert.AreEqual("Is beating the world best Go! player the greatest feet of man kind?", toCheck.Body);

            //reset edited post
            editedPost = new Post
            {
                PostId = 2,
                IsFeatured = false,
                ApprovalStatus = ApprovalEnums.Pending,
                Date = DateTime.Parse("6/5/2015"),
                Subject = "Google AI Project GO!",
                Body = "Is beating the world best Go! player the greatest feet of man kind?",
                Tags = _repo.GetAllTags().Where(t => t.TagId == 2).ToList(),
                UserId = userManager.FindByName("ConnieContributor").Id,
                User = userManager.FindByName("ConnieContributor")
            };

            _repo.EditPost(editedPost);
        }

		[Test]
		public void CreateAndRemoveTag()
		{
            List<Tag> tags = _repo.GetAllTags();
            int beforeCount = tags.Count;

			Tag tagToBeAdded = new Tag()
			{
				TagName = "FuturisticAI"
			};

			_repo.CreateTag(tagToBeAdded);

            List<Tag> tagsAfterCreate = _repo.GetAllTags();
            int afterCreateCount = tagsAfterCreate.Count;

            Assert.AreEqual(beforeCount + 1, afterCreateCount);

            _repo.RemoveTagById(tagToBeAdded.TagId);

            List<Tag> tagsAfterRemove = _repo.GetAllTags();
            int afterRemoveCount = tagsAfterRemove.Count();

            Assert.AreEqual(beforeCount, afterRemoveCount);
        }

        [Test]
		public void GetAllTags()
		{
            var tags = _repo.GetAllTags();

			Assert.AreEqual(4, tags.Count());
		}

		[Test]
		public void GetPostsBySearchTerm()
		{
			string searchTerm = "of";
			List<Post> returnedPosts = _repo.GetPostBySearchTerm(searchTerm);
			Assert.AreEqual(1, returnedPosts.Count());
			Assert.AreEqual("Deep learning is the process of using networks.....", returnedPosts[0].Body);
		}

		[Test]
		public void GetPostById()
		{
			Post getPost = _repo.GetPostById(3);
			Assert.AreEqual(3, getPost.PostId);
			Assert.AreEqual("Quantum computing, the needed power for AI", getPost.Subject);
		}

		[Test]
		public void GetPostsByPage()
		{
			List<Post> pagesOfPosts = _repo.GetApprovedPostByPageNumber(1);
			Assert.AreEqual(2, pagesOfPosts.Count());

            Assert.AreEqual(6, pagesOfPosts[0].PostId);

            pagesOfPosts = _repo.GetApprovedPostByPageNumber(2);
            Assert.AreEqual(0, pagesOfPosts.Count());
		}

		[Test]
		public void GetFeaturedPosts()
		{
			List<Post> featuredPosts = _repo.GetFeaturedPosts();

			Assert.AreEqual(2, featuredPosts.Count());
			Assert.AreEqual("Deep Learning", featuredPosts.Single(p => p.PostId == 1).Subject);
		}

		[Test]
        public void CanLoadAllStaticPages()
        {
            var staticPageCount = _repo.GetAllStaticPages();
            Assert.AreEqual(3, staticPageCount.Count());
        }

        [Test]
        public void CanLoadStaticPageById()
        {
			int pageId = 1;
            var page = _repo.GetStaticPageById(pageId);

            Assert.AreEqual(1, page.StaticPageId);
            Assert.AreEqual("Static Page", page.Subject);
            Assert.AreEqual("Static Page body.", page.Body);
            Assert.AreEqual("LinkName for static page 1", page.LinkName);

            int anotherPageId = 2;
            var anotherPage = _repo.GetStaticPageById(anotherPageId);

            Assert.AreEqual(2, anotherPage.StaticPageId);
            Assert.AreEqual("Static Page 2", anotherPage.Subject);
            Assert.AreEqual("Static Page body 2.", anotherPage.Body);
            Assert.AreEqual("LinkName for static page 2", anotherPage.LinkName);

            int nullPageId = 0;
            var nullPage = _repo.GetStaticPageById(nullPageId);

            Assert.AreEqual(null, nullPage);
        }

        [Test]
        public void CanSaveNewAndDeleteStaticPage()
        {
			StaticPage staticPage = new StaticPage();
            staticPage.LinkName = "The Link";
            staticPage.Subject = "Link";
            staticPage.Body = "This is a post about the Link";

            _repo.SaveNewStaticPage(staticPage);

            var staticPageCount = _repo.GetAllStaticPages();

            Assert.AreEqual(4, staticPageCount.Count());

            _repo.RemoveStaticPage(staticPage.StaticPageId);

            Assert.AreEqual(3, _repo.GetAllStaticPages().Count());
        }

        [Test]
        public void CanEditStaticPage()
        {
            StaticPage staticPage = new StaticPage();

            staticPage.StaticPageId = 2;
            staticPage.LinkName = "Updated Static Page";
            staticPage.Subject = "Updating the static page";
            staticPage.Body = "The static page has been updated";

            _repo.EditStaticPage(staticPage);

            var editedStatic = _repo.GetStaticPageById(2);

            Assert.AreEqual(2, editedStatic.StaticPageId);
            Assert.AreEqual("Updated Static Page", editedStatic.LinkName);
            Assert.AreEqual("Updating the static page", editedStatic.Subject);
            Assert.AreEqual("The static page has been updated", editedStatic.Body);

            staticPage.StaticPageId = 2;
            staticPage.LinkName = "LinkName for static page 2";
            staticPage.Subject = "Static Page 2";
            staticPage.Body = "Static Page body 2.";

            _repo.EditStaticPage(staticPage);

            editedStatic = _repo.GetStaticPageById(2);

            Assert.AreEqual(2, editedStatic.StaticPageId);
            Assert.AreEqual("LinkName for static page 2", editedStatic.LinkName);
            Assert.AreEqual("Static Page 2", editedStatic.Subject);
            Assert.AreEqual("Static Page body 2.", editedStatic.Body);
        }
        
        [Test]
        public void TagStringToTags()
        {
            string tagString = "NarrowAI other tags go here";
            var beforeCount = _repo.GetAllTags().Count();

            _repo.TagStringToTags(tagString);

            var afterCount = _repo.GetAllTags().Count();

            Assert.AreEqual(beforeCount + 4, afterCount);

            foreach(var item in _repo
                .GetAllTags()
                .Where(t => t.TagName == "other" || t.TagName == "tags" || t.TagName == "go" || t.TagName == "here").ToList())
            {
                _repo.RemoveTagById(item.TagId);
            }

            var finalCount = _repo.GetAllTags().Count();

            Assert.AreEqual(beforeCount, finalCount);
        }


    }
}
