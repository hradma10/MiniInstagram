// using Microsoft.EntityFrameworkCore;
// using MiniInstagramEF.context;
// using MiniInstagramEF.entities;

Console.WriteLine("");

// using var db = new MiniInstagramContext();
//
// db.Database.EnsureDeleted();
// db.Database.EnsureCreated();
//
// var u1 = new User { FirstName = "Adam", LastName = "Novák", Age = 25 };
// var u2 = new User { FirstName = "Bára", LastName = "Králová", Age = 22 };
// var u3 = new User { FirstName = "Cyril", LastName = "Svoboda", Age = 30 };
//
// db.Users.AddRange(u1, u2, u3);
//
// u1.Following.Add(u2);  
// u1.Following.Add(u3);   
//
// var p1 = new Post
// {
//     Author = u2,
//     Text = "Dnes jsem byla venku",
//     ImgPath = "img1.jpg",
//     ReleaseDate = DateTime.Now.AddDays(-10) 
// };
//
// var p2 = new Post
// {
//     Author = u2,
//     Text = "Já mám nový telefon!", 
//     ImgPath = "img2.jpg",
//     ReleaseDate = DateTime.Now.AddDays(-2)
// };
//
// var p3 = new Post
// {
//     Author = u3,
//     Text = "Skvělý den u vody",
//     ImgPath = "img3.jpg",
//     ReleaseDate = DateTime.Now.AddDays(-1)
// };
//
// var p4 = new Post
// {
//     Author = u3,
//     Text = "Já jsem programátor",
//     ImgPath = "img4.jpg",
//     ReleaseDate = DateTime.Now.AddDays(-3)
// };
//
// db.Posts.AddRange(p1, p2, p3, p4);
//
// p1.LikedBy.Add(u1);
// p1.LikedBy.Add(u2);
//
// p2.LikedBy.Add(u1);
//
// p3.LikedBy.Add(u1);
// p3.LikedBy.Add(u2);
//
// p4.LikedBy.Add(u3);
//
// db.SaveChanges();
//
// var weekAgo = DateTime.Now.AddDays(-7);
//
// var posts1 = db.Posts
//     .Where(p => p.AuthorId == u2.Id)
//     .Where(p => p.ReleaseDate < weekAgo)
//     .Where(p => !p.Text.ToLower().Contains("já"))
//     .ToList();
//
// Console.WriteLine("Posty uživatele Bára starší než týden bez slova já:");
// Console.WriteLine();
// foreach (var p in posts1) Console.WriteLine(p.Text);
// Console.WriteLine();
//
//
//
//
// var posts2 = db.Posts
//     .Where(p => p.LikedBy.Any(u => u.Id == u1.Id))
//     .ToList();
//
// Console.WriteLine("Posty lajknuté uživatelem Adam:");
// Console.WriteLine();
// foreach (var p in posts2) Console.WriteLine(p.Text);
// Console.WriteLine();
//
//
//
//
// var posts3 = db.Posts
//     .Where(p => p.LikedBy.Count() >= 2)
//     .ToList();
//
// Console.WriteLine("Posty lajknuté 2 nebo více uživateli:");
// Console.WriteLine();
// foreach (var p in posts3) Console.WriteLine(p.Text);
// Console.WriteLine();
//
// var followIds = u1.Following.Select(f => f.Id).ToList();
// var threeDaysAgo = DateTime.Now.AddDays(-3);
//
//
//
//
// var posts4 = db.Posts
//     .Where(p => followIds.Contains(p.AuthorId))
//     .Where(p => p.ReleaseDate >= threeDaysAgo)
//     .OrderByDescending(p => p.ReleaseDate)
//     .ToList();
//
// Console.WriteLine("Posty od sledovaných -> max 3 dny, seřazené od nejnovějších:");
// Console.WriteLine();
// foreach (var p in posts4) Console.WriteLine(p.Text);
// Console.WriteLine();