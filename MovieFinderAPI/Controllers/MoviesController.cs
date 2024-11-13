using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieFinderAPI.Data;
using MovieFinderAPI.Models;
using MovieFinderAPI.Models.DTO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;
namespace MovieFinderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieFinderContext _context;

        public MoviesController(MovieFinderContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        [HttpGet("{year}/{name}")]
        public async Task<ActionResult<Movie>> GetMovie(int year, string name)
        {
            var movie = await _context.Movies.FindAsync(year, name);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<MovieFinderUser>> login([FromBody] NewUser user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }
            var sql = "SELECT * FROM MovieFinderUser WHERE (Username = @p0 OR Email = @p0) AND Password = @p1";

            // Execute the query and map results to the entity
            var users = await _context.MovieFinderUsers
                .FromSqlRaw(sql, user.Username, user.Password)
                .ToListAsync();

            if (users.Count == 0)
            {
                return BadRequest("Invalid username or password");
            }
            else
            {
                // Map to DTO if necessary
                var userDto = new UserDTO
                {
                    Id = users[0].UserID,
                    Username = users[0].Username,
                    Email = users[0].Email,
                    FavouriteGenre = users[0].FavouriteGenre
                    // Map other properties as needed
                };

                return Ok(new {message="success", user=userDto });
            }
        }

            [HttpPost("Sign-Up")]
        public async Task<IActionResult> GetNextUserId([FromBody] NewUser user)
        {
            var sql = "SELECT NEXT VALUE FOR UserIDSequence;";
            try
            {
                var emailCheckSql = "SELECT COUNT(1) FROM MovieFinderUser WHERE Email = @Email";
                var emailParam = new SqlParameter("@Email", user.Email);

                await _context.Database.OpenConnectionAsync();
                using (var emailCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    emailCommand.CommandText = emailCheckSql;
                    emailCommand.Parameters.Add(emailParam);

                    var emailExists = (int)(await emailCommand.ExecuteScalarAsync()) > 0;

                    if (emailExists)
                    {
                        return BadRequest("The email is already in use.");
                    }
                }

                // Open a database connection and execute the SQL command
                await _context.Database.OpenConnectionAsync();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;

                    // Execute the command and get the result
                    var newUserId = (int)(await command.ExecuteScalarAsync());

                    var sqlInsertUser = @"
        INSERT INTO MovieFinderUser (UserID, Email, Username, Password, FavouriteGenre)
        VALUES (@UserID, @Email, @Username, @Password, @FavouriteGenre);";

                    var insertParams = new[]
                    {
                        new SqlParameter("@UserID", newUserId),
                        new SqlParameter("@Email", user.Email),
                        new SqlParameter("@Username", user.Username),
                        new SqlParameter("@Password", user.Password),
                        new SqlParameter("@FavouriteGenre", user.Genre)
                    };

                    using (var insertCommand = _context.Database.GetDbConnection().CreateCommand())
                    {
                        insertCommand.CommandText = sqlInsertUser;
                        insertCommand.Parameters.AddRange(insertParams);
                        await insertCommand.ExecuteNonQueryAsync();
                    }
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Success", Id = newUserId , Email = user.Email , username = user.Username , Genre = user.Genre });
                }
            }catch(Exception e)
            {
                return StatusCode(500, "Failed to create user");
            }
        }

        [HttpPost("searchActor/{actorname}")]
        public async Task<ActionResult<IEnumerable<string>>> GetActorLikeNames([FromRoute] string actorName)
        {
            if (string.IsNullOrWhiteSpace(actorName))
            {
                return BadRequest("Actor name cannot be empty.");
            }

            var sql = "SELECT ActorName FROM Actor WHERE ActorName LIKE @name";

            var nameParam = new SqlParameter("@name", $"%{actorName}%");

            var actorNames = new List<string>();

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.Add(nameParam);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            actorNames.Add(reader.GetString(0)); 
                        }
                    }
                }
            }

            // Return the list of actor names
            return Ok(actorNames);
        }


        [HttpPost("searchDirector/{directorName}")]
        public async Task<ActionResult<IEnumerable<string>>> GetDirectorLikeNames([FromRoute] string directorName)
        {
            if (string.IsNullOrWhiteSpace(directorName))
            {
                return BadRequest("Director name cannot be empty.");
            }

            var sql = "SELECT DirectorName FROM Director WHERE DirectorName LIKE @name";

            var nameParam = new SqlParameter("@name", $"%{directorName}%");

            var directorNames = new List<string>();

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.Add(nameParam);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            directorNames.Add(reader.GetString(0));
                        }
                    }
                }
            }

            // Return the list of director names
            return Ok(directorNames);
        }

        [HttpGet("getAllRatingCompanies")]
        public async Task<ActionResult<IEnumerable<RatingCompanyDto>>> GetAllRatingCompanies()
        {
            var sql = "SELECT RatingCompanyName, RatingScale FROM RatingCompany";

            var ratingCompanies = new List<RatingCompanyDto>();

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ratingCompanies.Add(new RatingCompanyDto
                            {
                                RatingCompanyName = reader.GetString(0),
                                RatingScale = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            // Return the list of rating companies with names and scales
            return Ok(ratingCompanies);
        }


        [HttpPost("moviesByFilters")]
        
        public async Task<ActionResult<MovieSearchDetails>> getFilteredMovie([FromBody] MovieFilters filters)
        {
            if (filters == null)
            {
                return null;
            }
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DurationMins", filters.DurationMins ?? (object)DBNull.Value),
                new SqlParameter("@MovieYear", filters.MovieYear ?? (object)DBNull.Value),
                new SqlParameter("@MovieTitle", filters.MovieTitle ?? (object)DBNull.Value),
                new SqlParameter("@ActorNames", filters.ActorNames ?? (object)DBNull.Value),
                new SqlParameter("@DirectorNames", filters.DirectorNames ?? (object)DBNull.Value),
                new SqlParameter("@StreamingPlatforms", filters.StreamingPlatforms ?? (object)DBNull.Value),
                new SqlParameter("@RatingCompanies", filters.RatingCompanies ?? (object)DBNull.Value),
                new SqlParameter("@Genres", filters.Genres ?? (object)DBNull.Value)
            };
            var movies = await _context.Set<MovieSearchDetails>()
                .FromSqlRaw("EXEC GetMoviesWithDetails @DurationMins, @MovieYear, @MovieTitle, @ActorNames, @DirectorNames, @StreamingPlatforms, @RatingCompanies, @Genres",
                            parameters.ToArray())
                .ToListAsync();
            if (movies == null || movies.Count == 0)
            {
                return null;
            }
            return Ok(movies);
        }




        [HttpGet("Start")]
        public async Task<IActionResult> PopulateDB()
        {
            string apiKey = "13671d5b04msha9c7b6c4161c378p1a13b7jsn8e4e4f768487";
            string apiHost = "streaming-availability.p.rapidapi.com";


        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-rapidapi-key", apiKey);
        client.DefaultRequestHeaders.Add("x-rapidapi-host", apiHost);

        string baseUrl = "https://streaming-availability.p.rapidapi.com/shows/search/filters";
        string country = "ca";
        string seriesGranularity = "show";
        string orderDirection = "asc";
        string orderBy = "original_title";
        string outputLanguage = "en";
        string showType = "movie";
        string nextCursor = null;
        bool hasMore = true;

        int movieLimit = 1000; // Set your desired movie limit
        int moviesProcessed = 0;

            try
            {
                do
                {
                    var requestUrl = "https://streaming-availability.p.rapidapi.com/shows/search/filters?country=ca&series_granularity=show&order_direction=asc&order_by=original_title&genres_relation=and&output_language=en&show_type=movie&rating_min=75";

                    if (!string.IsNullOrEmpty(nextCursor))
                    {
                        requestUrl += $"&cursor={Uri.EscapeDataString(nextCursor)}";
                    }

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(requestUrl),
                    };

                    HttpResponseMessage response = null;
                    bool success = false;

                    while (!success)
                    {
                        try
                        {
                            response = await client.SendAsync(request);

                            if (response.StatusCode == (HttpStatusCode)429)
                            {
                                // Rate limit exceeded
                                Console.WriteLine("Rate limit exceeded.");

                                // Try to get the Retry-After header
                                if (response.Headers.TryGetValues("Retry-After", out var retryAfterValues))
                                {
                                    var retryAfter = retryAfterValues.First();
                                    if (int.TryParse(retryAfter, out int delaySeconds))
                                    {
                                        Console.WriteLine($"Waiting for {delaySeconds} seconds before retrying...");
                                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                                    }
                                    else
                                    {
                                        // Retry-After is not an integer, wait for a default time
                                        Console.WriteLine("Invalid Retry-After value. Waiting for 10 seconds...");
                                        await Task.Delay(TimeSpan.FromSeconds(10));
                                    }
                                }
                                else
                                {
                                    // Retry-After header not provided, wait for a default time
                                    Console.WriteLine("Retry-After header not found. Waiting for 10 seconds...");
                                    await Task.Delay(TimeSpan.FromSeconds(10));
                                }

                                continue; // Retry the request
                            }

                            response.EnsureSuccessStatusCode();
                            success = true;
                        }
                        catch (HttpRequestException ex)
                        {
                            Console.WriteLine($"An HTTP error occurred: {ex.Message}");
                            Console.WriteLine("Waiting for 10 seconds before retrying...");
                            await Task.Delay(TimeSpan.FromSeconds(10));
                            // Continue the loop and retry
                        }
                    }

                    var body = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    // Deserialize the response into a container object
                    var apiResponseContainer = JsonSerializer.Deserialize<APIMovieResponseContainer>(body, options);

                    if (apiResponseContainer != null && apiResponseContainer.Shows != null && apiResponseContainer.Shows.Count > 0)
                    {
                        // Process each dirty movie
                        foreach (var dirtyMovie in apiResponseContainer.Shows)
                        {
                            if (moviesProcessed >= movieLimit)
                            {
                                hasMore = false;
                                return Ok($"Database populated successfully with {moviesProcessed} movies and reached its limit.");
                                break;
                            }

                            // Map dirty movie to clean movie using your Mapper class
                            var cleanMovie = Mapper.MapToAPIMovieResponse(dirtyMovie);

                            if (cleanMovie == null || cleanMovie.FirstAirYear == 0 || string.IsNullOrEmpty(cleanMovie.originalTitle))
                            {
                                continue; // Skip processing this movie
                            }

                            // Process and insert the movie along with related entities
                            await ProcessAndInsertMovieAsync(cleanMovie);
                            moviesProcessed++;
                        }

                        if (moviesProcessed >= movieLimit)
                        {
                            hasMore = false;
                            return Ok($"Database populated successfully with {moviesProcessed} movies and reached its limit.");
                            break;
                        }

                        nextCursor = apiResponseContainer.NextCursor;
                        hasMore = apiResponseContainer.HasMore;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    // Optional: Add a small delay between requests to be cautious
                    await Task.Delay(TimeSpan.FromSeconds(1));

                } while (hasMore);

                return Ok($"Database populated successfully with {moviesProcessed} movies.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        // -------------------------------------------
        private async Task PopulateDatabaseWithMovie(APIMovieResponseClean cleanMovie)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Ensure required fields are present
                if (cleanMovie.FirstAirYear == 0 || string.IsNullOrEmpty(cleanMovie.originalTitle))
                {
                    Console.WriteLine("Missing essential movie information. Skipping this movie.");
                    return;
                }

                // Check if the movie already exists
                var movieExists = await _context.Movies.AnyAsync(m => m.Year == cleanMovie.FirstAirYear && m.Name == cleanMovie.originalTitle);

                if (!movieExists)
                {
                    // Insert the movie
                    await _context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO Movie (year, Name, DurationMins, Description, Image) VALUES (@p0, @p1, @p2, @p3, @p4)",
                        cleanMovie.FirstAirYear,
                        cleanMovie.originalTitle,
                        cleanMovie.duration, // DurationMins is nullable
                        cleanMovie.overview ?? (object)DBNull.Value,
                        cleanMovie.verticalPosterW480 ?? (object)DBNull.Value
                    );
                }

                // Insert genres and link to the movie
                if (cleanMovie.genres != null)
                {
                    foreach (var genre in cleanMovie.genres)
                    {
                        // Insert into GenreToMovie
                        await _context.Database.ExecuteSqlRawAsync(
                            "INSERT INTO GenreToMovie (year, Name, Genre) VALUES (@p0, @p1, @p2)",
                            cleanMovie.FirstAirYear,
                            cleanMovie.originalTitle,
                            genre
                        );
                    }
                }

                // Insert directors and link to the movie
                if (cleanMovie.directors != null)
                {
                    foreach (var directorName in cleanMovie.directors)
                    {
                        // Check if director exists
                        var directorExists = await _context.Directors.AnyAsync(d => d.DirectorName == directorName);

                        if (!directorExists)
                        {
                            // Insert director
                            await _context.Database.ExecuteSqlRawAsync(
                                "INSERT INTO Director (DirectorName, Age, Gender) VALUES (@p0, @p1, @p2)",
                                directorName, null, null
                            );
                        }

                        // Insert into Directed_By
                        await _context.Database.ExecuteSqlRawAsync(
                            "INSERT INTO Directed_By (year, Name, DirectorName) VALUES (@p0, @p1, @p2)",
                            cleanMovie.FirstAirYear,
                            cleanMovie.originalTitle,
                            directorName
                        );
                    }
                }

                // Insert actors and link to the movie
                if (cleanMovie.cast != null)
                {
                    foreach (var actorName in cleanMovie.cast)
                    {
                        // Check if actor exists
                        var actorExists = await _context.Actors.AnyAsync(a => a.ActorName == actorName);

                        if (!actorExists)
                        {
                            // Insert actor
                            await _context.Database.ExecuteSqlRawAsync(
                                "INSERT INTO Actor (ActorName, Age, Gender) VALUES (@p0, @p1, @p2)",
                                actorName,null,null
                            );
                        }

                        // Insert into Acted_In
                        await _context.Database.ExecuteSqlRawAsync(
                            "INSERT INTO Acted_In (year, Name, ActorName) VALUES (@p0, @p1, @p2)",
                            cleanMovie.FirstAirYear,
                            cleanMovie.originalTitle,
                            actorName
                        );
                    }
                }

                // Insert streaming services and link to the movie
                if (cleanMovie.streamingServicesName != null)
                {
                    foreach (var serviceName in cleanMovie.streamingServicesName)
                    {
                        // Check if streaming service exists
                        var serviceExists = await _context.StreamingServices.AnyAsync(s => s.StreamingServiceName == serviceName);

                        if (!serviceExists)
                        {
                            // Insert streaming service
                            await _context.Database.ExecuteSqlRawAsync(
                                "INSERT INTO StreamingService (StreamingServiceName) VALUES (@p0)",
                                serviceName
                            );
                        }

                        // Insert into MovieStreamedOn
                        await _context.Database.ExecuteSqlRawAsync(
                            "INSERT INTO MovieStreamedOn (year, Name, StreamingServiceName) VALUES (@p0, @p1, @p2)",
                            cleanMovie.FirstAirYear,
                            cleanMovie.originalTitle,
                            serviceName
                        );
                    }
                }

                // Commit the transaction
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback the transaction on error
                await transaction.RollbackAsync();
                Console.WriteLine($"An error occurred while processing movie '{cleanMovie.originalTitle}': {ex.Message}");
            }
        }

        private async Task ProcessAndInsertMovieAsync(APIMovieResponseClean cleanMovie)
        {
            if (string.IsNullOrEmpty(cleanMovie.originalTitle))
            {
                // Cannot proceed without a valid movie name
                return;
            }
            // Check if the movie already exists
            var movieExists = await _context.Movies.AnyAsync(m => m.Year == cleanMovie.FirstAirYear && m.Name == cleanMovie.originalTitle);
            if (movieExists)
            {
                var movieEntity1 = new Movie
                {
                    Year = cleanMovie.FirstAirYear,
                    Name = cleanMovie.originalTitle,
                    DurationMins = cleanMovie.duration,
                    Description = cleanMovie.overview,
                    Image = cleanMovie.verticalPosterW480,
                    // Initialize navigation properties
                    ActedIns = new List<ActedIn>(),
                    DirectedBys = new List<DirectedBy>(),
                    ProducedBys = new List<ProducedBy>(),
                    MovieStreamedOns = new List<MovieStreamedOn>(),
                    // You might need to initialize other navigation properties
                };
                if (cleanMovie.creators != null && cleanMovie.creators.Count > 0)
                {
                    List<string> nameOfCreators = new List<string>();

                    foreach (var creatorName in cleanMovie.creators)
                    {
                        if (!nameOfCreators.Contains(creatorName) && creatorName != null)
                        {
                            // Check if the production company (creator) already exists
                            var companyExists = await _context.ProductionCompanies.AnyAsync(pc => pc.ProductionCompanyName == creatorName);
                            ProductionCompany productionCompany;

                            if (companyExists)
                            {
                                productionCompany = await _context.ProductionCompanies.FirstAsync(pc => pc.ProductionCompanyName == creatorName);
                            }
                            else
                            {
                                productionCompany = new ProductionCompany { ProductionCompanyName = creatorName };
                                _context.ProductionCompanies.Add(productionCompany);
                            }

                            // Check if the ProducedBy relationship already exists
                            var producedByExists = await _context.ProducedBys
                                .AnyAsync(pb => pb.Year == movieEntity1.Year && pb.Name == movieEntity1.Name && pb.ProductionCompanyName == creatorName);

                            if (!producedByExists)
                            {
                                // Create the ProducedBy relationship if it doesn't exist
                                var producedBy = new ProducedBy
                                {
                                    Year = movieEntity1.Year,
                                    Name = movieEntity1.Name,
                                    ProductionCompanyName = creatorName,
                                    ProductionCompany = productionCompany,
                                    Movie = movieEntity1
                                };
                                _context.ProducedBys.Add(producedBy);
                            }

                            // Add the creator to the list to avoid redundant checks within this loop
                            nameOfCreators.Add(creatorName);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(cleanMovie.rating.ToString()))
                {
                    // Check if "IMDB" exists in RatingCompany table, add if not
                    var ratingCompany = await _context.RatingCompanies
                        .SingleOrDefaultAsync(rc => rc.RatingCompanyName == "IMDB");

                    if (ratingCompany == null)
                    {
                        ratingCompany = new RatingCompany
                        {
                            RatingCompanyName = "IMDB",
                            RatingScale = "varies" // or any appropriate scale description
                        };
                        _context.RatingCompanies.Add(ratingCompany);
                        
                    }
                    var ratingExists = await _context.MovieRatings
                        .AnyAsync(r => r.Year == movieEntity1.Year && r.Name == movieEntity1.Name && r.RatingCompanyName == "IMDB");
                    if (!ratingExists)
                    {
                        // Create the MovieRating for IMDB
                        var movieRating = new MovieRating
                        {
                            Year = movieEntity1.Year,
                            Name = movieEntity1.Name,
                            RatingCompanyName = "IMDB",
                            Score = cleanMovie.rating.ToString(),
                            Summary = "IMDB rating"
                        };
                        _context.MovieRatings.Add(movieRating);

                    }
                }
                await _context.SaveChangesAsync();
                return;
            }

            // Create the Movie entity
            var movieEntity = new Movie
            {
                Year = cleanMovie.FirstAirYear,
                Name = cleanMovie.originalTitle,
                DurationMins = cleanMovie.duration,
                Description = cleanMovie.overview,
                Image = cleanMovie.verticalPosterW480,
                // Initialize navigation properties
                ActedIns = new List<ActedIn>(),
                DirectedBys = new List<DirectedBy>(),
                ProducedBys = new List<ProducedBy>(),
                MovieStreamedOns = new List<MovieStreamedOn>(),
                // You might need to initialize other navigation properties
            };

            // Process Genres
            if (cleanMovie.genres != null && cleanMovie.genres.Count > 0)
            {
                foreach (var genreName in cleanMovie.genres)
                {

                    // Create the relationship
                    var movieGenre = new GenreToMovie
                    {
                        Year = movieEntity.Year,
                        Name = movieEntity.Name,
                        Genre = genreName,
                        Movie = movieEntity
                    };
                    _context.GenreToMovies.Add(movieGenre);
                }
            }

            // Process Actors
            if (cleanMovie.cast != null && cleanMovie.cast.Count > 0)
            {
                List<String> nameOfActors = [];
                foreach (var actorName in cleanMovie.cast)
                {

                    // Check if the actor already exists
                    if (!nameOfActors.Contains(actorName) && actorName !=null) { 
                   
                    var actorExists = await _context.Actors.AnyAsync(a => a.ActorName == actorName);
                    Actor actor;
                    if (actorExists)
                    {
                        actor = await _context.Actors.FirstAsync(a => a.ActorName == actorName);
                    }
                    else
                    {
                        actor = new Actor { ActorName = actorName };
                        _context.Actors.Add(actor);
                    }

                    // Create the relationship
                    var actedIn = new ActedIn
                    {
                        Year = movieEntity.Year,
                        Name = movieEntity.Name,
                        ActorName = actorName,
                        Actor = actor,
                        Movie = movieEntity
                    };
                    _context.ActedIns.Add(actedIn);
                        nameOfActors.Add(actorName);
                    }



                }
            }

            // Process Directors
            if (cleanMovie.directors != null && cleanMovie.directors.Count > 0)
            {
                List<String> nameOfDirector = [];

                foreach (var directorName in cleanMovie.directors)
                {

                    if (!nameOfDirector.Contains(directorName) && directorName != null)
                    {
                        // Check if the director already exists
                        var directorExists = await _context.Directors.AnyAsync(d => d.DirectorName == directorName);
                        Director director;
                        if (directorExists)
                        {
                            director = await _context.Directors.FirstAsync(d => d.DirectorName == directorName);
                        }
                        else
                        {
                            director = new Director { DirectorName = directorName };
                            _context.Directors.Add(director);
                        }

                        // Create the relationship
                        var directedBy = new DirectedBy
                        {
                            Year = movieEntity.Year,
                            Name = movieEntity.Name,
                            DirectorName = directorName,
                            Director = director,
                            Movie = movieEntity
                        };
                        _context.DirectedBys.Add(directedBy);
                        nameOfDirector.Add(directorName);

                    }

                }
            }



            


            // Process Streaming Services
            if (cleanMovie.streamingServicesName != null && cleanMovie.streamingServicesName.Count > 0)
            {
                List<String> nameOfService = [];
                foreach (var serviceName in cleanMovie.streamingServicesName)
                {
                    if (!nameOfService.Contains(serviceName) && serviceName != null)
                    {
                        // Check if the streaming service already exists
                        var serviceExists = await _context.StreamingServices.AnyAsync(s => s.StreamingServiceName == serviceName);
                        StreamingService service;
                        if (serviceExists)
                        {
                            service = await _context.StreamingServices.FirstAsync(s => s.StreamingServiceName == serviceName);
                        }
                        else
                        {
                            service = new StreamingService { StreamingServiceName = serviceName };
                            _context.StreamingServices.Add(service);
                        }

                        // Create the relationship
                        var movieStreamedOn = new MovieStreamedOn
                        {
                            Year = movieEntity.Year,
                            Name = movieEntity.Name,
                            StreamingServiceName = serviceName,
                            StreamingService = service,
                            Movie = movieEntity
                        };
                        _context.MovieStreamedOns.Add(movieStreamedOn);
                        nameOfService.Add(serviceName);
                    }
                }
            }
           

            // Add the movie to the context
            _context.Movies.Add(movieEntity);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }


        //---------------------------------------------

        [HttpPost("addGenre")]
        public async Task<ActionResult> AddGenre(GenreToMovie dto)
        {
            var result = await _context.Database.ExecuteSqlRawAsync("INSERT INTO GenreToMovie(year, Name, Genre) VALUES(@p0, @p1, @p2)", dto.Year, dto.Name, dto.Genre);

            if (result > 0)
            {
                return Ok("Genre added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the genre.");
            }
        }

        [HttpGet("actorExists")]
        public async Task<int> ActorExists(string actor)
        {
            var exists = await _context.Database.ExecuteSqlRawAsync(
                "SELECT CASE WHEN EXISTS (SELECT 1 FROM Actor a WHERE a.ActorName = @p0) THEN 1 ELSE 0 END",
                actor);
            return exists;
        }

        [HttpPost("addActor")]
        public async Task<ActionResult> AddActor(string actor)
        {
            var result = await _context.Database.ExecuteSqlRawAsync("INSERT INTO Actor(ActorName, Age ,Gender)VALUES(@p0,NULL,NULL)", actor);
            if (result > 0)
            {
                return Ok("Actor added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the actor.");
            }
        }

        [HttpPost("addDirector")]
        public async Task<ActionResult> AddDirector(string directorName, int? age = null, string gender = null)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "INSERT INTO Director (DirectorName, Age, Gender) VALUES (@p0, @p1, @p2)",
                directorName, age.HasValue ? (object)age.Value : DBNull.Value, (object)gender ?? DBNull.Value);

            if (result > 0)
            {
                return Ok("Director added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the director.");
            }
        }

        [HttpPost("addDirectedBy")]
        public async Task<ActionResult> AddDirectedBy(int year, string movieName, string directorName)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "INSERT INTO Directed_By (year, Name, DirectorName) VALUES (@p0, @p1, @p2)",
                year, movieName, directorName);

            if (result > 0)
            {
                return Ok("Directed_By relationship added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the relationship.");
            }
        }

        [HttpPost("addProductionCompany")]
        public async Task<ActionResult> AddProductionCompany(string companyName)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "INSERT INTO ProductionCompany (ProductionCompanyName) VALUES (@p0)",
                companyName);

            if (result > 0)
            {
                return Ok("Production company added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the production company.");
            }
        }

        [HttpPost("addProducedBy")]
        public async Task<ActionResult> AddProducedBy(int year, string movieName, string companyName)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "INSERT INTO Produced_By (year, Name, ProductionCompanyName) VALUES (@p0, @p1, @p2)",
                year, movieName, companyName);

            if (result > 0)
            {
                return Ok("Produced_By relationship added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the relationship.");
            }
        }

        [HttpPost("addStreamingService")]
        public async Task<ActionResult> AddStreamingService(string serviceName)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "INSERT INTO StreamingService (StreamingServiceName) VALUES (@p0)",
                serviceName);

            if (result > 0)
            {
                return Ok("Streaming service added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the streaming service.");
            }
        }

        [HttpPost("addMovieStreamedOn")]
        public async Task<ActionResult> AddMovieStreamedOn(int year, string movieName, string serviceName)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "INSERT INTO MovieStreamedOn (year, Name, StreamingServiceName) VALUES (@p0, @p1, @p2)",
                year, movieName, serviceName);

            if (result > 0)
            {
                return Ok("MovieStreamedOn relationship added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the relationship.");
            }
        }

        [HttpPut("BIGTEST")]
        public async Task<ActionResult> addSomeBullShit()
        {
            try
            {
                var movieRating = new MovieRating
                {
                    Year = 1000,
                    Name = "movieEntity1.Name",
                    RatingCompanyName = "IMDB",
                    Score = 88.ToString(),
                    Summary = "IMDB rating"
                };
                _context.MovieRatings.Add(movieRating);
                await _context.SaveChangesAsync();
                return Ok();
               


             
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            

        }

    }
}