USE AIBlog;
GO

SET NOCOUNT ON
PRINT 'Deleting StaticPages'
DELETE FROM dbo.StaticPages
PRINT 'Deleting TagPosts'
DELETE FROM TagPosts
PRINT 'Deleting Tags'
DELETE FROM Tags
PRINT 'Deleting Posts'
DELETE FROM Posts

DBCC CHECKIDENT(StaticPages, RESEED, 0)
DBCC CHECKIDENT(Posts, RESEED, 0)
DBCC CHECKIDENT(Tags, RESEED, 0)
GO


PRINT 'Inserting StaticPages'
INSERT INTO StaticPages
VALUES ('Static Page','Static Page body.','LinkName for static page 1'),
('Static Page 2','Static Page body 2.','LinkName for static page 2'),
('Static Page 3','Static Page body 3.','LinkName for static page 3')
GO


PRINT 'Inserting Tags'
INSERT INTO Tags
VALUES ('DeepLearning'),
('NarrowAI'),
('BroadAI'),
('Inspiration')


PRINT 'Inserting Posts'
DECLARE @AdamId NVARCHAR(128)
DECLARE @ConnieId  NVARCHAR(128)

SELECT @AdamId = Id FROM AspNetUsers where UserName = 'AdamAdmin'
SELECT @ConnieId = Id FROM AspNetUsers where UserName = 'ConnieContributor'

--approved = 0
--pending  = 1
--rejected = 2
INSERT INTO Posts
--IsFeatured, ApprovalStatus, Date, Subject, Body, UserId
VALUES ('1','0','2015-06-01','Deep Learning','Deep learning is the process of using networks.....',@AdamId),
('0','1','2015-06-05','Google AI Project GO!','Is beating the world best Go! player the greatest feet of man kind?',@ConnieId),
('0','2','2015-06-07','Quantum computing, the needed power for AI','Is quantum computing practical, or will it be to fragile to ever use in practice?',@AdamId),
('0','1','2015-06-08','Another post by user 3','This is yet another post by user 3',@ConnieId),
('0','1','2017-07-10','Ideas wanted','I need ideas for more posts - Michael',@AdamId),
('1','0','2017-07-19','Inspiration','"When life gives you lemons, don''t make lemonade. Make life take the lemons back! Get mad! I don''t want your damn lemons, what the hell am I supposed to do with these? Demand to see life''s manager! Make life rue the day it thought it could give Cave Johnson lemons! Do you know who I am? I''m the man who''s gonna burn your house down! With the lemons! I''m gonna get my engineers to invent a combustible lemon that burns your house down!― J.K. Simmons',@AdamId),
('0','2','2020-01-01','Failure of a post','This is a bad post',@AdamId)


PRINT 'Inserting TagPosts'
INSERT INTO TagPosts
SELECT T.TagId, P.PostId FROM Posts p
CROSS JOIN Tags t
WHERE P.PostId = 1

INSERT INTO TagPosts
VALUES (2,2),
(1,3),
(2,4),
(3,5),
(4,6),
(4,7)
SET NOCOUNT OFF

/*
SELECT * FROM Posts
SELECT * FROM Tags
SELECT * FROM TagPosts
SELECT * FROM StaticPages
*/