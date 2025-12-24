// using Microsoft.AspNetCore.Http;

// namespace cdn.Handlers.Post;

// public class UploadHandler
// {
//     private const string STORAGE_ROOT = "/storage";

//     public UploadHandler(string aStorageRoot)
//     {
//     }

//     public async Task<IResult> Post(IFormFile file)
//     {
//         if (file == null || file.Length == 0)
//         {
//             return Results.BadRequest("No file uploaded.");
//         }

//         // Prevent path traversal
//         var fileName = Path.GetFileName(file.FileName);
//         if (string.IsNullOrWhiteSpace(fileName))
//         {
//             return Results.BadRequest("Invalid file name.");
//         }

//         var storagePath = Path.Combine(STORAGE_ROOT, fileName);

//         Directory.CreateDirectory(STORAGE_ROOT);

//         await using (var stream = File.Create(storagePath))
//         {
//             await file.CopyToAsync(stream);
//         }

//         return Results.Ok(new
//         {
//             name = fileName,
//             size = file.Length
//         });
//     }
// }
