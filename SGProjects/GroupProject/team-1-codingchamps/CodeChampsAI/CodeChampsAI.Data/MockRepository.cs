using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeChampsAI.Models;
using CodeChampsAI.Models.Identity;
using CodeChampsAI.Data.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace CodeChampsAI.Data
{
	public class MockRepository : IRepository
	{
        static RoleStore<AppRole> roleStore = new RoleStore<AppRole>(new ApplicationDbContext());
        static UserManager<AppUser> userManager = new UserManager<AppUser>(new UserStore<AppUser>(new ApplicationDbContext()));

        private int postPerPage = 5;

        #region _StaticPages
        private static List<StaticPage> _StaticPages = new List<StaticPage>
        {
            new StaticPage
            {
                StaticPageId = 1,
                Subject = "Static Page",
                Body = "Static Page body.",
                LinkName = "LinkName for static page 1"
            },
            new StaticPage
            {
                StaticPageId = 2,
                Subject = "Static Page 2",
                Body = "Static Page body 2.",
                LinkName = "LinkName for static page 2"
            },
            new StaticPage
            {
                StaticPageId = 3,
                Subject = "Static Page 3",
                Body = "Static Page body 3.",
                LinkName = "LinkName for static page 3"
            },
        };
        #endregion

        #region _Tags
        private static List<Tag> _Tags = new List<Tag>
        {
            new Tag
            {
                TagName = "DeepLearning",
                TagId = 1,
            },
            new Tag
            {
                TagName = "NarrowAI",
                TagId = 2,
            },
            new Tag
            {
                TagName = "BroadAI",
                TagId = 3,
            },
            new Tag
            {
                TagName = "Inspiration",
                TagId = 4
            }
        };
        #endregion

        #region _Posts
        private static List<Post> _Posts = new List<Post>
        {
            new Post
            {
                PostId = 1,
                IsFeatured = true,
                ApprovalStatus = ApprovalEnums.Approved,
                Date = DateTime.Parse("6/1/2015"),
                Subject = "Deep Learning",
                Body = "Deep learning is the process of using networks.....",
                Tags = _Tags,
                UserId = userManager.FindByName("AdamAdmin").Id,
                User = userManager.FindByName("AdamAdmin")
            },
            new Post
            {
                PostId = 2,
                IsFeatured = false,
                ApprovalStatus = ApprovalEnums.Pending,
                Date = DateTime.Parse("6/5/2015"),
                Subject = "Google AI Project GO!",
                Body = "Is beating the world best Go! player the greatest feet of man kind?",
                Tags = _Tags.Where(x => x.TagId == 2).ToList(),
                UserId = userManager.FindByName("ConnieContributor").Id,
                User = userManager.FindByName("ConnieContributor")
            },
            new Post
            {
                PostId = 3,
                IsFeatured = false,
                ApprovalStatus = ApprovalEnums.Rejected,
                Date = DateTime.Parse("6/7/2015"),
                Subject = "Quantum computing, the needed power for AI",
                Body = "Is quantum computing practical, or will it be to fragile to ever use in practice?",
                Tags = _Tags.Where(x => x.TagId == 1).ToList(),
                UserId = userManager.FindByName("AdamAdmin").Id,
                User = userManager.FindByName("AdamAdmin")
            },

            new Post
            {
                PostId = 4,
                IsFeatured = false,
                ApprovalStatus = ApprovalEnums.Pending,
                Date = DateTime.Parse("6/8/2015"),
                Subject = "Another post by user 3",
                Body = "This is yet another post by user 3",
                Tags = _Tags.Where(x => x.TagId == 2).ToList(),
                UserId = userManager.FindByName("ConnieContributor").Id,
                User = userManager.FindByName("ConnieContributor")
            },

            new Post
            {
                PostId = 5,
                IsFeatured = false,
                ApprovalStatus = ApprovalEnums.Pending,
                Date = DateTime.Parse("7/10/17"),
                Subject = "Ideas wanted",
                Body = "I need ideas for more posts - Michael",
                Tags = _Tags.Where(x => x.TagId == 3).ToList(),
                UserId = userManager.FindByName("AdamAdmin").Id,
                User = userManager.FindByName("AdamAdmin")
            },

            new Post
            {
                PostId = 6,
                IsFeatured = true,
                ApprovalStatus = ApprovalEnums.Approved,
                Date = DateTime.Parse("7/19/17"),
                Subject = "Inspiration",
                Body = "When life gives you lemons, don't make lemonade. Make life take the lemons back! Get mad! I don't want your damn lemons, what the hell am I supposed to do with these? Demand to see life's manager! Make life rue the day it thought it could give Cave Johnson lemons! Do you know who I am? I'm the man who's gonna burn your house down! With the lemons! I'm gonna get my engineers to invent a combustible lemon that burns your house down!― J.K. Simmons",
                Tags = _Tags.Where(x => x.TagId == 4).ToList(),
                UserId = userManager.FindByName("AdamAdmin").Id,
                User = userManager.FindByName("AdamAdmin")
            },

            new Post
            {
                PostId = 7,
                IsFeatured = false,
                ApprovalStatus = ApprovalEnums.Rejected,
                Date = DateTime.Parse("1/1/2020"),
                Subject = "Failure of a post",
                Body = "This is a bad post",
                Tags = _Tags.Where(x => x.TagId == 4).ToList(),
                UserId = userManager.FindByName("AdamAdmin").Id,
                User = userManager.FindByName("AdamAdmin")
            }
        };
        #endregion

        public void SaveNewPost(Post postToBeSaved)
		{
			if (_Posts.SingleOrDefault(x => x.PostId == postToBeSaved.PostId) == null)
			{
				postToBeSaved.PostId = _Posts.Max(x => x.PostId) + 1;
				_Posts.Add(postToBeSaved);
			}
		}

		public IEnumerable<Post> GetPendingPosts(string userName)
		{
            IEnumerable<Post> posts = new List<Post>();
            if (userName == null)
            {
                posts = _Posts.Where(x => x.ApprovalStatus == ApprovalEnums.Pending);
            }
            else
            {
                posts = _Posts.Where(x => x.User.UserName == userName).Where(y => y.ApprovalStatus == ApprovalEnums.Pending);
            }

            return posts;
        }

		public void EditPost(Post editedPost)
		{
			var oldPost = _Posts.SingleOrDefault(x => x.PostId == editedPost.PostId);

			oldPost.PostId = editedPost.PostId;
			oldPost.IsFeatured = editedPost.IsFeatured;
			oldPost.Subject = editedPost.Subject;
			oldPost.Tags = editedPost.Tags;
			oldPost.UserId = editedPost.UserId;
			oldPost.ApprovalStatus = editedPost.ApprovalStatus;
			oldPost.Body = editedPost.Body;
			oldPost.Date = editedPost.Date;
			oldPost.Tags = editedPost.Tags;
		}

        public List<Post> GetPostsByUserName(string username)
        {
            var userId = userManager.FindByName(username).Id;

            List<Post> posts = _Posts.Where(x => x.UserId == userId).ToList();

            return posts;
        }

		public void DeletePost(Post postToBeDeleted)
		{
			if (_Posts.SingleOrDefault(x => x.PostId == postToBeDeleted.PostId) != null)
			{
				_Posts.Remove(_Posts.SingleOrDefault(x => x.PostId == postToBeDeleted.PostId));
			}
		}

		public void CreateTag(Tag tagToBeCreated)
		{
			if (_Tags.SingleOrDefault(x => x.TagId == tagToBeCreated.TagId) == null)
			{
				tagToBeCreated.TagId = _Tags.Max(x => x.TagId) + 1;
				_Tags.Add(tagToBeCreated);
			}
		}

		public List<Post> GetPostBySearchTerm(string searchTerm)
		{
			List<Post> postWithSearchTerm = new List<Post>();
            if(searchTerm == null)
            {
                searchTerm = "";
            }
			postWithSearchTerm = _Posts
                .Where(x => x.Body.Contains(searchTerm)
                            && x.ApprovalStatus == ApprovalEnums.Approved)
                .ToList();
			return postWithSearchTerm;
		}

		public List<Post> GetPostByTagId(int? tagId)
		{
            List<Post> posts = new List<Post>();
            foreach(var x in _Posts)
            {
                if (x.Tags.Any(y => y.TagId == tagId))
                {
                    posts.Add(x);
                }
            }
            return posts;
		}

		public Post GetPostById(int postId)
		{
			return _Posts.SingleOrDefault(x => x.PostId == postId);
		}

		public List<Post> GetApprovedPostByPageNumber(int pageNumber)
		{
			int startingPost = (pageNumber * postPerPage)-postPerPage;
			int endingPost = startingPost;
			return _Posts
                .OrderByDescending(p => p.Date)
                .Where(x => x.ApprovalStatus == ApprovalEnums.Approved)
                .Skip(startingPost)
                .Take(5)
                .ToList();
		}

        public List<Post> GetApprovedPostByPageNumberContributor(int pageNumber, string userName)
        {
            int startingPost = (pageNumber * postPerPage) - postPerPage;
            int endingPost = startingPost;
            int postsTaken = 0;
            int postsToTake = postPerPage;
            List<Post> posts = new List<Post>();
            var totalPosts = _Posts.Where(x => x.User.UserName == userName).Where(x => x.ApprovalStatus == ApprovalEnums.Approved).Count();
            if (totalPosts < postPerPage * pageNumber)
            {
                postsTaken = totalPosts - (postPerPage * pageNumber);
            }
            posts = _Posts.Skip(startingPost).Where(x => x.User.UserName == userName).Where(x => x.ApprovalStatus == ApprovalEnums.Approved).Take(postsToTake + postsTaken).ToList();
            return posts;
        }

        public List<Post> GetFeaturedPosts()
		{
			return _Posts.Where(x => x.IsFeatured == true).ToList();
		}

        public List<StaticPage> GetAllStaticPages()
        {
            return _StaticPages;
        }

        public StaticPage GetStaticPageById(int pageId)
        {
            StaticPage page = _StaticPages.FirstOrDefault(x => x.StaticPageId == pageId);

            return page;
        }

        public void SaveNewStaticPage(StaticPage staticPage)
        {
            staticPage.StaticPageId = _StaticPages.Max(x => x.StaticPageId) + 1;

            _StaticPages.Add(staticPage);
        }

        public void EditStaticPage(StaticPage staticPage)
        {
            Parallel.ForEach(_StaticPages.Where(x => x.StaticPageId == staticPage.StaticPageId), y =>
            {
                y.StaticPageId = staticPage.StaticPageId;
                y.LinkName = staticPage.LinkName;
                y.Subject = staticPage.Subject;
                y.Body = staticPage.Body;
            });
        }

        public void RemoveStaticPage(int staticPageId)
        {
            _StaticPages.RemoveAll(x => x.StaticPageId == staticPageId);
        }

        public int GetTotalNumberOfPages()
        {
            int totalPages = _Posts.Where(x => x.ApprovalStatus == ApprovalEnums.Approved).Count() / postPerPage;
            if(_Posts.Where(x => x.ApprovalStatus == ApprovalEnums.Approved).Count() % postPerPage != 0)
            {
                totalPages++;
            }
            return totalPages;
        }

        public int GetTotalNumberOfPagesContributor(string userName)
        {
            int totalPages = _Posts.Where(x => x.User.UserName == userName).Where(x => x.ApprovalStatus == ApprovalEnums.Approved).Count() / postPerPage;
            if (_Posts.Where(x => x.User.UserName == userName).Where(x => x.ApprovalStatus == ApprovalEnums.Approved).Count() % postPerPage != 0)
            {
                totalPages++;
            }
            return totalPages;
        }

        public List<AppUser> GetUsersByPage(int pageNumber = 1)
        {
            var roleId = roleStore.Roles.SingleOrDefault(r => r.Name == "Disabled").Id;
            int skip = (pageNumber * postPerPage) - postPerPage;
            return userManager
                .Users
                .Where(u => u.Roles.All(r => r.RoleId != roleId))
                .OrderBy(u => u.UserName)
                .Skip(skip)
                .Take(postPerPage)
                .ToList();
        }

        public List<AppRole> GetUserRoles()
        {
            return roleStore.Roles.Where(r => r.Name != "Disabled").ToList();
        }

        public List<Post> SearchResults(int? id, string searchTerm)
        {
            var model = GetPostBySearchTerm(searchTerm)
                .Skip((id.Value - 1) * postPerPage)
                .Take(postPerPage)
                .ToList();
            return model;
        }

        public List<Post> GetAllPosts()
        {            
            return _Posts;
        }

		public IEnumerable<Post> GetRejectedPosts(string userName)
		{
            IEnumerable<Post> posts = new List<Post>();
            if(userName == null)
            {
                posts = _Posts.Where(x => x.ApprovalStatus == ApprovalEnums.Rejected);
            }
            else
            {
                posts = _Posts.Where(x => x.User.UserName == userName).Where(y => y.ApprovalStatus == ApprovalEnums.Rejected);
            }

            return posts;
		}

        public void UpdateUserRole(string username, string role)
        {
            var user = userManager.FindByName(username);
            if(user != null && user.Roles.Count > 0)
            {
                foreach(var userrole in roleStore.Roles.Select(r => r.Name))
                {
                    if(userManager.IsInRole(user.Id, userrole))
                    {
                        userManager.RemoveFromRole(user.Id, userrole);
                    }
                }
            }

            if (user != null)
            {
                userManager.AddToRole(user.Id, role);
            }
        }

        public void DeleteUser(string username)
        {
            var user = userManager.FindByName(username);

            if (user != null && user.Roles.Count > 0)
            {
                foreach (var userrole in roleStore.Roles.Select(r => r.Name))
                {
                    if (userManager.IsInRole(user.Id, userrole))
                    {
                        userManager.RemoveFromRole(user.Id, userrole);
                    }
                }
            }
            if (user != null && !userManager.IsInRole(user.Id, "Disabled"))
            {
                userManager.AddToRole(user.Id, "Disabled");
            }
        }

        public int MaxUserPages()
        {
            var roleId = roleStore.Roles.SingleOrDefault(r => r.Name == "Disabled").Id;

            int userCount = userManager
                .Users
                .Where(u => u.Roles.All(r => r.RoleId != roleId))
                .Count();

            return (userCount / postPerPage) + (userCount % postPerPage == 0 ? 0 : 1);
        }

        public List<Tag> TagStringToTags(string tagString)
        {
            var tagSplit = tagString.Replace('#', ' ').Split(new char[] {' '} , StringSplitOptions.RemoveEmptyEntries);
            List<Tag> tagsToReturn = new List<Tag>();

            foreach(var item in tagSplit)
            {
                var singleTag = _Tags.SingleOrDefault(t => t.TagName.ToLower() == item.ToLower());
                if(singleTag == null)
                {
                    Tag tagToCreate = new Tag { TagName = item };
                    CreateTag(tagToCreate);
                    tagsToReturn.Add(tagToCreate);
                }
                else
                {
                    tagsToReturn.Add(singleTag);
                }
            }

            return tagsToReturn;
        }

        public List<Tag> GetAllTags()
        {
            return _Tags;
        }

        public void RemoveTagById(int tagId)
        {
            _Tags.Remove(_Tags.Single(t => t.TagId == tagId));
        }

        public void ApprovePost(int id)
        {
            var post = _Posts.FirstOrDefault(x => x.PostId == id);
            post.ApprovalStatus = ApprovalEnums.Approved;
        }

        public void RejectPost(int id)
        {
            var post = _Posts.FirstOrDefault(x => x.PostId == id);
            post.ApprovalStatus = ApprovalEnums.Rejected;
        }

        public void ToggleFeatured(int id)
        {
            var post = _Posts.FirstOrDefault(x => x.PostId == id);
            if (post.IsFeatured)
            {
                post.IsFeatured = false;
            }
            else
            {
                post.IsFeatured = true;
            }

        }

        public int GetPostMaxPagesBySearchTerm(string searchTerm)
        {
            int postCount = GetPostBySearchTerm(searchTerm).Count();
            int pages = (postCount / postPerPage) + (postCount % postPerPage == 0 ? 0 : 1);
            return pages;
        }
    }
}
