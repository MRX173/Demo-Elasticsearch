using Application.Abstract;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _ctx;

        public PostRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Post> AddAsync(Post post)
        {
            await _ctx.Post.AddAsync(post);
            await _ctx.SaveChangesAsync();
            return post;
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = await GetByIdAsync(id);
            _ctx.Post.Remove(post);
            await _ctx.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _ctx.Post.ToListAsync();
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            return await _ctx.Post.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Post post)
        {
            _ctx.Post.Update(post);
            await _ctx.SaveChangesAsync();
        }
    }
}
