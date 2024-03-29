﻿using DevGames.API.Entities;
using DevGames.API.Models;
using DevGames.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevGames.API.Controllers
{
    [Route("api/boards/{id}/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DevGamesContext context;

        public PostsController(DevGamesContext context)
        {
            this.context = context;
        }

        // api/boards/1/posts
        [HttpGet]
        public IActionResult GetAll(int id)
        {
            var board = context.Boards.SingleOrDefault(b => b.Id == id);

            if (board == null)
                return NotFound();

            return Ok(board.Posts);
        }

        [HttpGet("{postId}")]
        public IActionResult GetById(int id, int postId)
        {
            var board = context.Boards.SingleOrDefault(b => b.Id == id);

            if (board == null)
                return NotFound();

            var post = board.Posts.SingleOrDefault(p => p.Id == postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
        public IActionResult Post(int id, AddPostInputModel model)
        {
            var board = context.Boards.SingleOrDefault(b => b.Id == id);

            if (board == null)
                return NotFound();

            var post = new Post(model.Id, model.Title, model.Description);

            board.AddPost(post);

            return CreatedAtAction(nameof(GetById), new { id = id, postId = post.Id }, model);
        }

        // api/boards/1/posts/1/comments POST
        [HttpPost("{postId}/comments")]
        public IActionResult PostComment(int id, int postId, AddCommentInputModel model)
        {
            var board = context.Boards.SingleOrDefault(b => b.Id == id);

            if (board == null)
                return NotFound();

            var post = board.Posts.SingleOrDefault(p => p.Id == postId);

            if (post == null)
                return NotFound();

            var comment = new Comment(model.Title, model.Description, model.User);

            post.AddComment(comment);

            return NoContent();
        }
    }
}
