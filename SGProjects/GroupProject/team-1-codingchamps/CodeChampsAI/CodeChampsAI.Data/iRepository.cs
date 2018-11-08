using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeChampsAI.Models;
using CodeChampsAI.Models.Identity;

namespace CodeChampsAI.Data
{
	public interface IRepository
	{
		void SaveNewPost(Post postToBeSaved);
		void EditPost(Post editedPost);
		List<Post> GetPostsByUserName(string username);
		void DeletePost(Post postToBeDeleted);
		void CreateTag(Tag tagToBeCreated);
        void RemoveTagById(int tagId);
		List<Post> GetPostBySearchTerm(string searchTerm);
        List<Post> SearchResults(int? id, string searchTerm);
        List<Post> GetPostByTagId(int? tagId);
		Post GetPostById(int postId);
        List<Post> GetAllPosts();
        List<Post> GetApprovedPostByPageNumber(int pageNumber);
        List<Post> GetApprovedPostByPageNumberContributor(int pageNumber, string userName);
        List<Post> GetFeaturedPosts();
		List<StaticPage> GetAllStaticPages();
		StaticPage GetStaticPageById(int pageId);
		void SaveNewStaticPage(StaticPage staticPage);
		void EditStaticPage(StaticPage staticPage);
		void RemoveStaticPage(int staticPageId);
        void UpdateUserRole(string username, string role);
        IEnumerable<Post> GetPendingPosts(string userName);
        List<AppUser> GetUsersByPage(int pageNumber = 1);
        List<AppRole> GetUserRoles();
        void DeleteUser(string username);
        int GetPostMaxPagesBySearchTerm(string searchTerm);
        int MaxUserPages();
        List<Tag> TagStringToTags(string tagString);
        List<Tag> GetAllTags();
        int GetTotalNumberOfPages();
        int GetTotalNumberOfPagesContributor(string userName);
        IEnumerable<Post> GetRejectedPosts(string userName);
		void ApprovePost(int id);
		void RejectPost(int id);
		void ToggleFeatured(int id);
	}
}
