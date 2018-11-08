using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeChampsAI.Data.Identity;
using CodeChampsAI.Models;
using CodeChampsAI.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CodeChampsAI.Data
{

	public class EFRepo : IRepository
	{
		static RoleStore<AppRole> roleStore = new RoleStore<AppRole>(new ApplicationDbContext());
		static UserManager<AppUser> userManager = new UserManager<AppUser>(new UserStore<AppUser>(new ApplicationDbContext()));

		private int postPerPage = 5;


		public void ApprovePost(int id)
		{
			using (var repository = new ApplicationDbContext())
			{
				Post post = repository.Posts.FirstOrDefault(x => x.PostId == id);
				post.ApprovalStatus = ApprovalEnums.Approved;
				repository.SaveChanges();
			}
		}

		public void CreateTag(Tag tagToBeCreated)
		{
			using (var repository = new ApplicationDbContext())
			{
				if (repository.Tags.SingleOrDefault(x => x.TagId == tagToBeCreated.TagId) == null)
				{
					repository.Tags.Add(tagToBeCreated);
					repository.SaveChanges();
				}
			}
		}

		public void DeletePost(Post postToBeDeleted)
		{
			using (var repository = new ApplicationDbContext())
			{
				if (repository.Posts.SingleOrDefault(x => x.PostId == postToBeDeleted.PostId) != null)
				{
                    var actualPostToDelete = repository.Posts.SingleOrDefault(x => x.PostId == postToBeDeleted.PostId);

                    actualPostToDelete.Tags.RemoveAll(t => t.TagId == t.TagId);

					repository.Posts.Remove(actualPostToDelete);
				}
				repository.SaveChanges();
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

		public void EditPost(Post editedPost)
		{
			using (var repository = new ApplicationDbContext())
			{
				var oldPost = repository.Posts.SingleOrDefault(x => x.PostId == editedPost.PostId);


				oldPost.IsFeatured = editedPost.IsFeatured;
				oldPost.Subject = editedPost.Subject;
				oldPost.ApprovalStatus = editedPost.ApprovalStatus;
				oldPost.Body = editedPost.Body;               

                oldPost.Tags.RemoveAll(t => !editedPost.Tags.Any(t2 => t2.TagId == t.TagId));

                foreach(var item in editedPost.Tags.Where(t => !oldPost.Tags.Any(t2 => t2.TagId == t.TagId)))
                {
                    oldPost.Tags.Add(repository.Tags.Attach(item));
                }
				repository.SaveChanges();
			}
		}

		public void EditStaticPage(StaticPage staticPage)
		{
			using (var repository = new ApplicationDbContext())
			{
				Parallel.ForEach(repository.StaticPages.Where(x => x.StaticPageId == staticPage.StaticPageId), y =>
				{
					y.StaticPageId = staticPage.StaticPageId;
					y.LinkName = staticPage.LinkName;
					y.Subject = staticPage.Subject;
					y.Body = staticPage.Body;
				});
				repository.SaveChanges();
			}
		}

		public List<Post> GetAllPosts()
		{
			using (var repository = new ApplicationDbContext())
			{
				return repository.Posts.ToList();
			}
		}

		public List<StaticPage> GetAllStaticPages()
		{
			using (var repository = new ApplicationDbContext())
			{
				return repository.StaticPages.ToList();

			}
		}

		public List<Tag> GetAllTags()
		{
			using (var repository = new ApplicationDbContext())
			{
				return repository.Tags.ToList();
			}
		}

		public List<Post> GetApprovedPostByPageNumber(int pageNumber)
		{
			using (var repository = new ApplicationDbContext())
			{
				int startingPost = (pageNumber * postPerPage) - postPerPage;
				int endingPost = startingPost;
				return repository
                    .Posts
                    .OrderByDescending(p => p.Date)
                    .Where(x => x.ApprovalStatus == ApprovalEnums.Approved)
                    .Skip(startingPost)
                    .Take(5)
                    .Include(p => p.Tags)
                    .Include(p => p.User)
                    .ToList();
			}
		}

        public List<Post> GetApprovedPostByPageNumberContributor(int pageNumber, string userName)
        {
            using (var repository = new ApplicationDbContext())
            {
                int startingPost = (pageNumber * postPerPage) - postPerPage;
                int endingPost = startingPost;
                return repository
                    .Posts.Where(x => x.User.UserName == userName)
                    .OrderByDescending(p => p.Date)
                    .Where(x => x.ApprovalStatus == ApprovalEnums.Approved)
                    .Skip(startingPost)
                    .Take(5)
                    .Include(p => p.Tags)
                    .Include(p => p.User)
                    .ToList();
            }
        }

        public List<Post> GetFeaturedPosts()
		{
			using (var repository = new ApplicationDbContext())
			{
				return repository
                    .Posts
                    .Where(x => x.IsFeatured == true)
                    .Include(p => p.User)
                    .Include(p => p.Tags)
                    .ToList();
			}
		}

		public IEnumerable<Post> GetPendingPosts(string userName)
		{
			using (var repository = new ApplicationDbContext())
			{
				IEnumerable<Post> posts = new List<Post>();
				if (userName == null)
				{
					posts = repository
                        .Posts
                        .Where(x => x.ApprovalStatus == ApprovalEnums.Pending)
                        .OrderBy(p => p.Date)
                        .Include(p => p.User)
                        .ToList();
				}
				else
				{
					posts = repository
                        .Posts
                        .Where(x => x.User.UserName == userName)
                        .Where(y => y.ApprovalStatus == ApprovalEnums.Pending)
                        .Include(p => p.User)
                        .ToList();
				}

				return posts;
			}
		}

		public Post GetPostById(int postId)
		{
			using (var repository = new ApplicationDbContext())
			{
				return repository
                    .Posts
                    .Include(p => p.Tags)
                    .Include(p => p.User)
                    .SingleOrDefault(x => x.PostId == postId);
			}
		}

		public List<Post> GetPostBySearchTerm(string searchTerm)
		{
			using (var repository = new ApplicationDbContext())
			{
				List<Post> postWithSearchTerm = new List<Post>();
				if (searchTerm == null)
				{
					searchTerm = "";
				}
				postWithSearchTerm = repository
                    .Posts
                    .Where(x => x.Body.Contains(searchTerm) && x.ApprovalStatus == ApprovalEnums.Approved)
                    .Include(p => p.User)
                    .Include(p => p.Tags)
                    .ToList();
				return postWithSearchTerm;
			}
		}

		public List<Post> GetPostByTagId(int? tagId)
		{
            List<Post> posts = new List<Post>();

            using (var repository = new ApplicationDbContext())
			{
				foreach (var x in repository.Posts.Include(p => p.User).ToList())
				{
					if (x.Tags.Any(y => y.TagId == tagId))
					{
						posts.Add(x);
					}
				}
			}

            return posts;

        }

        public int GetPostMaxPagesBySearchTerm(string searchTerm)
        {
            int postCount = GetPostBySearchTerm(searchTerm).Count();
            int pages = (postCount / postPerPage) + (postCount % postPerPage == 0 ? 0 : 1);
            return pages;
        }

        public List<Post> GetPostsByUserName(string username)
		{
			using (var repository = new ApplicationDbContext())
			{
				var userId = userManager.FindByName(username).Id;
				List<Post> posts = repository.Posts.Where(x => x.UserId == userId).ToList();
				return posts;
			}
		}

		public IEnumerable<Post> GetRejectedPosts(string userName)
		{
			using (var repository = new ApplicationDbContext())
			{
				IEnumerable<Post> posts = new List<Post>();
				if (userName == null)
				{
					posts = repository
                        .Posts
                        .Where(x => x.ApprovalStatus == ApprovalEnums.Rejected)
                        .OrderBy(p => p.Date)
                        .Include(p => p.User)
                        .ToList();
				}
				else
				{
					posts = repository
                        .Posts
                        .Where(x => x.User.UserName == userName)
                        .Where(y => y.ApprovalStatus == ApprovalEnums.Rejected)
                        .Include(p => p.User)
                        .ToList();
				}

				return posts;
			}
		}

		public StaticPage GetStaticPageById(int pageId)
		{
			using (var repository = new ApplicationDbContext())
			{
				StaticPage page = repository.StaticPages.FirstOrDefault(x => x.StaticPageId == pageId);
				return page;
			}
		}

		public int GetTotalNumberOfPages()
		{
			using (var repository = new ApplicationDbContext())
			{
                int totalPosts = repository
                    .Posts
                    .Count(p => p.ApprovalStatus == ApprovalEnums.Approved);

                int totalPages = totalPosts / postPerPage;
				if (totalPosts % postPerPage != 0)
				{
					totalPages++;
				}
				return totalPages;

			}
		}

        public int GetTotalNumberOfPagesContributor(string userName)
        {
            using (var repository = new ApplicationDbContext())
            {
                int totalPosts = repository
                    .Posts
                    .Count(p => p.ApprovalStatus == ApprovalEnums.Approved
                                && p.User.UserName == userName);

                int totalPages = totalPosts / postPerPage;
                if (repository.Posts.Count() % postPerPage != 0)
                {
                    totalPages++;
                }
                return totalPages;

            }
        }

        public List<AppRole> GetUserRoles()
		{
			using (var repository = new ApplicationDbContext())
			{
				return roleStore.Roles.Where(r => r.Name != "Disabled").ToList();
			}
		}

		public List<AppUser> GetUsersByPage(int pageNumber = 1)
		{
			using (var repository = new ApplicationDbContext())
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
		}

		public int MaxUserPages()
		{
			using (var repository = new ApplicationDbContext())
			{
				var roleId = roleStore.Roles.SingleOrDefault(r => r.Name == "Disabled").Id;

				int userCount = userManager
					.Users
					.Where(u => u.Roles.All(r => r.RoleId != roleId))
					.Count();

				return (userCount / postPerPage) + (userCount % postPerPage == 0 ? 0 : 1);
			}
		}

		public void RejectPost(int id)
		{
			using (var repository = new ApplicationDbContext())
			{
				var post = repository.Posts.FirstOrDefault(x => x.PostId == id);
				post.ApprovalStatus = ApprovalEnums.Rejected;
				repository.SaveChanges();
			}
		}

		public void RemoveStaticPage(int staticPageId)
		{
			using (var repository = new ApplicationDbContext())
			{
				var removePage = repository.StaticPages.SingleOrDefault(x => x.StaticPageId == staticPageId);
				repository.StaticPages.Remove(removePage);
				repository.SaveChanges();
			}
		}

		public void RemoveTagById(int tagId)
		{
			using (var repository = new ApplicationDbContext())
			{
				repository.Tags.Remove(repository.Tags.Single(t => t.TagId == tagId));
				repository.SaveChanges();
			}
		}

		public void SaveNewPost(Post postToBeSaved)
		{
			using (var repository = new ApplicationDbContext())
			{
				if (repository.Posts.SingleOrDefault(x => x.PostId == postToBeSaved.PostId) == null)
				{
                    Post test = new Post
                    {
                        ApprovalStatus = postToBeSaved.ApprovalStatus,
                        Body = postToBeSaved.Body,
                        Date = postToBeSaved.Date,
                        IsFeatured = postToBeSaved.IsFeatured,
                        Subject = postToBeSaved.Subject,
                        User = repository.Users.Single(u => u.UserName == postToBeSaved.User.UserName),
                        UserId = repository.Users.Single(u => u.UserName == postToBeSaved.User.UserName).Id,
                        Tags = new List<Tag>()
                    };
                    foreach (var item in postToBeSaved.Tags)
                    {
                        test.Tags.Add(repository.Tags.Single(t => t.TagId == item.TagId));
                    }

                    repository.Posts.Add(test);
					repository.SaveChanges();
                    postToBeSaved.PostId = test.PostId;
				}
			}
		}

		public void SaveNewStaticPage(StaticPage staticPage)
		{
			using (var repository = new ApplicationDbContext())
			{
				repository.StaticPages.Add(staticPage);
				repository.SaveChanges();
			}
		}

		public List<Post> SearchResults(int? id, string searchTerm)
		{
			using (var repository = new ApplicationDbContext())
			{
				var model = GetPostBySearchTerm(searchTerm)
                    .Skip((id.Value - 1) * postPerPage)
                    .Take(postPerPage)
                    .ToList();
				return model;
			}
		}

		public List<Tag> TagStringToTags(string tagString)
		{
            List<Tag> tagsToReturn = new List<Tag>();

            using (var repository = new ApplicationDbContext())
			{
				var tagSplit = tagString.Replace('#', ' ').Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				foreach (var item in tagSplit)
				{
					var singleTag = repository.Tags.SingleOrDefault(t => t.TagName.ToLower() == item.ToLower());
					if (singleTag == null)
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
			}
            return tagsToReturn.ToList();
        }

        public void ToggleFeatured(int id)
		{
			using (var repository = new ApplicationDbContext())
			{
				var post = repository.Posts.FirstOrDefault(x => x.PostId == id);
				if (post.IsFeatured)
				{
					post.IsFeatured = false;
				}
				else
				{
					post.IsFeatured = true;
				}
				repository.SaveChanges();
			}
		}

		public void UpdateUserRole(string username, string role)
		{
			using (var repository = new ApplicationDbContext())
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

				if (user != null)
				{
					userManager.AddToRole(user.Id, role);
				}
				repository.SaveChanges();
			}
		}
	}
}
