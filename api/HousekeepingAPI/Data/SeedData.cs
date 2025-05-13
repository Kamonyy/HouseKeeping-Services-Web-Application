using HousekeepingAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HousekeepingAPI.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure database is created and migrations are applied
            await context.Database.MigrateAsync();

            // Clean up empty categories before starting
            var emptyInitialCategories = await context.Categories.Where(c => string.IsNullOrWhiteSpace(c.Name)).ToListAsync();
            if (emptyInitialCategories.Any())
            {
                Console.WriteLine($"Removing {emptyInitialCategories.Count} empty categories");
                context.Categories.RemoveRange(emptyInitialCategories);
                await context.SaveChangesAsync();
            }

            // Seed roles
            if (!await context.Roles.AnyAsync())
            {
                await EnsureRoleCreated(roleManager, "Admin");
                await EnsureRoleCreated(roleManager, "Provider");
                await EnsureRoleCreated(roleManager, "User");
            }

            // Seed users - store references for later use in relationships
            AppUser? adminUser = null;
            AppUser? provider = null;
            AppUser? customer = null;

            if (!await context.Users.AnyAsync())
            {
                adminUser = new AppUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "مدير",
                    LastName = "النظام",
                    EmailConfirmed = true,
                    UserType = UserType.Admin
                };
                await CreateUser(userManager, adminUser, "Kamony123!@#", "Admin");

                provider = new AppUser
                {
                    UserName = "provider@example.com",
                    Email = "provider@example.com",
                    FirstName = "أحمد",
                    LastName = "محمود",
                    EmailConfirmed = true,
                    UserType = UserType.Provider
                };
                await CreateUser(userManager, provider, "Kamony123!@#", "Provider");

                customer = new AppUser
                {
                    UserName = "user@example.com",
                    Email = "user@example.com",
                    FirstName = "محمد",
                    LastName = "علي",
                    EmailConfirmed = true,
                    UserType = UserType.Customer
                };
                await CreateUser(userManager, customer, "Kamony123!@#", "User");
            }
            else
            {
                // Get existing users for relationships
                adminUser = await userManager.FindByEmailAsync("admin@example.com");
                provider = await userManager.FindByEmailAsync("provider@example.com");
                customer = await userManager.FindByEmailAsync("user@example.com");
            }

            // Add more users
            AppUser? extraProvider = null;
            AppUser? extraCustomer = null;
            if (await context.Users.AllAsync(u => u.UserName != "extra.provider@example.com"))
            {
                extraProvider = new AppUser
                {
                    UserName = "extra.provider@example.com",
                    Email = "extra.provider@example.com",
                    FirstName = "سارة",
                    LastName = "حسن",
                    EmailConfirmed = true,
                    UserType = UserType.Provider
                };
                await CreateUser(userManager, extraProvider, "Kamony123!@#", "Provider");
            }
            else
            {
                extraProvider = await userManager.FindByEmailAsync("extra.provider@example.com");
            }
            if (await context.Users.AllAsync(u => u.UserName != "extra.customer@example.com"))
            {
                extraCustomer = new AppUser
                {
                    UserName = "extra.customer@example.com",
                    Email = "extra.customer@example.com",
                    FirstName = "ليلى",
                    LastName = "سعيد",
                    EmailConfirmed = true,
                    UserType = UserType.Customer
                };
                await CreateUser(userManager, extraCustomer, "Kamony123!@#", "User");
            }
            else
            {
                extraCustomer = await userManager.FindByEmailAsync("extra.customer@example.com");
            }

            // Seed Categories - track them for relationships
            Dictionary<string, Category> categoryDict = new Dictionary<string, Category>();

            if (!await context.Categories.AnyAsync())
            {
                var categoryList = new List<Category>
                {
                    new Category { Name = "تنظيف" },
                    new Category { Name = "صيانة" },
                    new Category { Name = "تركيب" },
                    new Category { Name = "كهرباء" }
                };

                await context.Categories.AddRangeAsync(categoryList);
                await context.SaveChangesAsync();

                // Store categories in dictionary for easy lookup
                foreach (var category in categoryList)
                {
                    categoryDict[category.Name] = category;
                }
            }
            else
            {
                // Clean up empty categories
                var emptyInitialCategories2 = await context.Categories.Where(c => string.IsNullOrWhiteSpace(c.Name)).ToListAsync();
                if (emptyInitialCategories2.Any())
                {
                    context.Categories.RemoveRange(emptyInitialCategories2);
                    await context.SaveChangesAsync();
                }

                // Load existing categories into dictionary
                var existingCategories = await context.Categories.ToListAsync();
                foreach (var category in existingCategories)
                {
                    if (!string.IsNullOrWhiteSpace(category.Name))
                    {
                        categoryDict[category.Name] = category;
                    }
                }
            }

            // Ensure we have all required categories
            if (!categoryDict.ContainsKey("تنظيف"))
            {
                var category = new Category { Name = "تنظيف" };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                categoryDict["تنظيف"] = category;
            }
            if (!categoryDict.ContainsKey("صيانة"))
            {
                var category = new Category { Name = "صيانة" };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                categoryDict["صيانة"] = category;
            }
            if (!categoryDict.ContainsKey("تركيب"))
            {
                var category = new Category { Name = "تركيب" };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                categoryDict["تركيب"] = category;
            }
            if (!categoryDict.ContainsKey("كهرباء"))
            {
                var category = new Category { Name = "كهرباء" };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                categoryDict["كهرباء"] = category;
            }

            // Drop all existing subcategories to avoid any relationship issues
            var existingSubCategories = await context.SubCategories.ToListAsync();
            if (existingSubCategories.Any())
            {
                // First remove any service-subcategory relationships
                var existingServiceSubcategories = await context.ServiceSubCategories.ToListAsync();
                if (existingServiceSubcategories.Any())
                {
                    context.ServiceSubCategories.RemoveRange(existingServiceSubcategories);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Removed {existingServiceSubcategories.Count} service-subcategory relationships");
                }
                
                // Now it's safe to remove the subcategories
                context.SubCategories.RemoveRange(existingSubCategories);
                await context.SaveChangesAsync();
                Console.WriteLine($"Removed {existingSubCategories.Count} existing subcategories for clean recreation");
            }

            // Seed SubCategories with proper relationships to Categories
            Dictionary<string, SubCategory> subCategoryDict = new Dictionary<string, SubCategory>();

            // Create subcategory objects
            var subCategories = new List<SubCategory>
            {
                // Cleaning subcategories
                new SubCategory { 
                    Name = "تنظيف منازل", 
                    CategoryId = categoryDict["تنظيف"].Id,
                    Category = categoryDict["تنظيف"]
                },
                new SubCategory { 
                    Name = "تنظيف سجاد", 
                    CategoryId = categoryDict["تنظيف"].Id,
                    Category = categoryDict["تنظيف"]
                },
                
                // Maintenance subcategories
                new SubCategory { 
                    Name = "صيانة تكييف", 
                    CategoryId = categoryDict["صيانة"].Id,
                    Category = categoryDict["صيانة"]
                },
                new SubCategory { 
                    Name = "سباكة", 
                    CategoryId = categoryDict["صيانة"].Id,
                    Category = categoryDict["صيانة"]
                },
                
                // Installation subcategories
                new SubCategory { 
                    Name = "تركيب أثاث", 
                    CategoryId = categoryDict["تركيب"].Id,
                    Category = categoryDict["تركيب"]
                },
                new SubCategory { 
                    Name = "تركيب ستائر", 
                    CategoryId = categoryDict["تركيب"].Id,
                    Category = categoryDict["تركيب"]
                },
                
                // Electrical subcategories
                new SubCategory { 
                    Name = "إصلاح كهرباء", 
                    CategoryId = categoryDict["كهرباء"].Id,
                    Category = categoryDict["كهرباء"]
                },
                new SubCategory { 
                    Name = "تركيب إضاءة", 
                    CategoryId = categoryDict["كهرباء"].Id,
                    Category = categoryDict["كهرباء"]
                }
            };

            // Add subcategories to database
            await context.SubCategories.AddRangeAsync(subCategories);
            await context.SaveChangesAsync();
            Console.WriteLine($"Created {subCategories.Count} subcategories with proper category relationships");

            // Store subcategories in dictionary for later use
            foreach (var subCategory in subCategories)
            {
                subCategoryDict[subCategory.Name] = subCategory;
            }

            // Make sure we have the required subcategories for our services
            string[] requiredSubcats = { "تنظيف منازل", "صيانة تكييف", "تركيب أثاث" };
            foreach (var subcatName in requiredSubcats)
            {
                // These should already exist, but double-check
                if (!subCategoryDict.ContainsKey(subcatName))
                {
                    string categoryName = "";
                    if (subcatName.Contains("تنظيف")) categoryName = "تنظيف";
                    else if (subcatName.Contains("صيانة")) categoryName = "صيانة";
                    else if (subcatName.Contains("تركيب")) categoryName = "تركيب";
                    else categoryName = categoryDict.Keys.First(); // fallback
                    
                    if (categoryDict.ContainsKey(categoryName))
                    {
                        var subCategory = new SubCategory { 
                            Name = subcatName, 
                            CategoryId = categoryDict[categoryName].Id,
                            Category = categoryDict[categoryName]
                        };
                        await context.SubCategories.AddAsync(subCategory);
                        await context.SaveChangesAsync();
                        subCategoryDict[subcatName] = subCategory;
                    }
                }
            }

            // Seed Services with proper provider relationship
            Dictionary<string, Models.Service> serviceDict = new Dictionary<string, Models.Service>();

            // Drop existing services and related data
            var existingServices = await context.Services.ToListAsync();
            if (existingServices.Any())
            {
                // First remove any comments
                var existingComments = await context.Comments.ToListAsync();
                if (existingComments.Any())
                {
                    context.Comments.RemoveRange(existingComments);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Removed {existingComments.Count} existing comments");
                }
                
                // Now remove services
                context.Services.RemoveRange(existingServices);
                await context.SaveChangesAsync();
                Console.WriteLine($"Removed {existingServices.Count} existing services for clean recreation");
            }

            // Create new services if we have a provider user
            if (provider != null)
            {
                var services = new List<Models.Service>
                {
                    new Models.Service
                    {
                        Title = "خدمة تنظيف منازل",
                        Description = "خدمة شاملة لتنظيف المنازل تشمل الأرضيات والحمامات والمطبخ",
                        EstimatedPrice = 50000m,
                        ContactPhone = "07701234567",
                        UserId = provider.Id,
                        Provider = provider,
                        IsApproved = true,
                        CreatedDate = DateTime.UtcNow.AddDays(-5)
                    },
                    new Models.Service
                    {
                        Title = "صيانة تكييف",
                        Description = "خدمة صيانة وتنظيف أجهزة التكييف المركزي والسبليت",
                        EstimatedPrice = 30000m,
                        ContactPhone = "07701234567",
                        UserId = provider.Id,
                        Provider = provider,
                        IsApproved = true,
                        CreatedDate = DateTime.UtcNow.AddDays(-3)
                    },
                    new Models.Service
                    {
                        Title = "تركيب أثاث",
                        Description = "تركيب جميع أنواع الأثاث المنزلي",
                        EstimatedPrice = 25000m,
                        ContactPhone = "07701234567",
                        UserId = provider.Id,
                        Provider = provider,
                        IsApproved = true,
                        CreatedDate = DateTime.UtcNow.AddDays(-1)
                    }
                };

                await context.Services.AddRangeAsync(services);
                await context.SaveChangesAsync();
                Console.WriteLine($"Created {services.Count} services");

                foreach (var service in services)
                {
                    serviceDict[service.Title] = service;
                }

                // Create relationships between services and subcategories
                Console.WriteLine("Creating service-subcategory relationships...");
                
                if (subCategoryDict.ContainsKey("تنظيف منازل") && serviceDict.ContainsKey("خدمة تنظيف منازل"))
                {
                    var serviceSubCategory = new ServiceSubCategory
                    {
                        ServiceId = serviceDict["خدمة تنظيف منازل"].Id,
                        Service = serviceDict["خدمة تنظيف منازل"],
                        SubCategoryId = subCategoryDict["تنظيف منازل"].Id,
                        SubCategory = subCategoryDict["تنظيف منازل"]
                    };
                    await context.ServiceSubCategories.AddAsync(serviceSubCategory);
                    Console.WriteLine("- Created relationship: خدمة تنظيف منازل -> تنظيف منازل");
                }
                else 
                {
                    Console.WriteLine("- WARNING: Could not create relationship for خدمة تنظيف منازل");
                }

                if (subCategoryDict.ContainsKey("صيانة تكييف") && serviceDict.ContainsKey("صيانة تكييف"))
                {
                    var serviceSubCategory = new ServiceSubCategory
                    {
                        ServiceId = serviceDict["صيانة تكييف"].Id,
                        Service = serviceDict["صيانة تكييف"],
                        SubCategoryId = subCategoryDict["صيانة تكييف"].Id,
                        SubCategory = subCategoryDict["صيانة تكييف"]
                    };
                    await context.ServiceSubCategories.AddAsync(serviceSubCategory);
                    Console.WriteLine("- Created relationship: صيانة تكييف -> صيانة تكييف");
                }
                else 
                {
                    Console.WriteLine("- WARNING: Could not create relationship for صيانة تكييف");
                }

                if (subCategoryDict.ContainsKey("تركيب أثاث") && serviceDict.ContainsKey("تركيب أثاث"))
                {
                    var serviceSubCategory = new ServiceSubCategory
                    {
                        ServiceId = serviceDict["تركيب أثاث"].Id,
                        Service = serviceDict["تركيب أثاث"],
                        SubCategoryId = subCategoryDict["تركيب أثاث"].Id,
                        SubCategory = subCategoryDict["تركيب أثاث"]
                    };
                    await context.ServiceSubCategories.AddAsync(serviceSubCategory);
                    Console.WriteLine("- Created relationship: تركيب أثاث -> تركيب أثاث");
                }
                else 
                {
                    Console.WriteLine("- WARNING: Could not create relationship for تركيب أثاث");
                }

                await context.SaveChangesAsync();
            } 
            else 
            {
                Console.WriteLine("WARNING: Could not create services - provider user not found");
            }

            // Seed Comments with proper relationships
            if (customer != null && serviceDict.Count > 0)
            {
                var comments = new List<Comment>();
                Console.WriteLine("Creating comments...");
                
                if (serviceDict.ContainsKey("خدمة تنظيف منازل"))
                {
                    comments.Add(new Comment
                    {
                        Content = "خدمة ممتازة، شكراً جزيلاً",
                        UserId = customer.Id,
                        User = customer,
                        ServiceId = serviceDict["خدمة تنظيف منازل"].Id,
                        Service = serviceDict["خدمة تنظيف منازل"],
                        CreatedDate = DateTime.UtcNow.AddDays(-1)
                    });
                    Console.WriteLine("- Created comment for خدمة تنظيف منازل");
                }
                
                if (serviceDict.ContainsKey("صيانة تكييف"))
                {
                    comments.Add(new Comment
                    {
                        Content = "تم الإصلاح بسرعة وبجودة عالية",
                        UserId = customer.Id,
                        User = customer,
                        ServiceId = serviceDict["صيانة تكييف"].Id,
                        Service = serviceDict["صيانة تكييف"],
                        CreatedDate = DateTime.UtcNow.AddDays(-1)
                    });
                    Console.WriteLine("- Created comment for صيانة تكييف");
                }
                
                if (comments.Count > 0)
                {
                    await context.Comments.AddRangeAsync(comments);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Added {comments.Count} comments");
                }
            }
            else
            {
                Console.WriteLine("WARNING: Could not create comments - customer user or services not found");
            }

            // Add more services for extraProvider
            if (extraProvider != null)
            {
                var extraServices = new List<Models.Service>
                {
                    new Models.Service
                    {
                        Title = "تنظيف مكاتب",
                        Description = "تنظيف شامل للمكاتب والشركات.",
                        EstimatedPrice = 60000m,
                        ContactPhone = "07709876543",
                        UserId = extraProvider.Id,
                        Provider = extraProvider,
                        IsApproved = true,
                        CreatedDate = DateTime.UtcNow.AddDays(-2)
                    },
                    new Models.Service
                    {
                        Title = "تركيب أجهزة كهربائية",
                        Description = "تركيب وصيانة الأجهزة الكهربائية المنزلية.",
                        EstimatedPrice = 40000m,
                        ContactPhone = "07709876543",
                        UserId = extraProvider.Id,
                        Provider = extraProvider,
                        IsApproved = false,
                        CreatedDate = DateTime.UtcNow.AddDays(-1)
                    }
                };
                await context.Services.AddRangeAsync(extraServices);
                await context.SaveChangesAsync();
                foreach (var service in extraServices)
                {
                    serviceDict[service.Title] = service;
                }
            }

            // Add more comments from extraCustomer
            if (extraCustomer != null && serviceDict.Count > 0)
            {
                var extraComments = new List<Comment>();
                if (serviceDict.ContainsKey("تنظيف مكاتب"))
                {
                    extraComments.Add(new Comment
                    {
                        Content = "خدمة رائعة وسريعة!",
                        UserId = extraCustomer.Id,
                        User = extraCustomer,
                        ServiceId = serviceDict["تنظيف مكاتب"].Id,
                        Service = serviceDict["تنظيف مكاتب"],
                        CreatedDate = DateTime.UtcNow.AddHours(-10)
                    });
                }
                if (serviceDict.ContainsKey("تركيب أجهزة كهربائية"))
                {
                    extraComments.Add(new Comment
                    {
                        Content = "تم التركيب بشكل ممتاز.",
                        UserId = extraCustomer.Id,
                        User = extraCustomer,
                        ServiceId = serviceDict["تركيب أجهزة كهربائية"].Id,
                        Service = serviceDict["تركيب أجهزة كهربائية"],
                        CreatedDate = DateTime.UtcNow.AddHours(-5)
                    });
                }
                if (extraComments.Count > 0)
                {
                    await context.Comments.AddRangeAsync(extraComments);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Added {extraComments.Count} extra comments");
                }
            }

            // --- BEGIN: Ensure each subcategory has at least two services and each service has at least two comments, with context-aware data ---
            var allProviders = new List<AppUser>();
            if (provider != null) allProviders.Add(provider);
            if (extraProvider != null) allProviders.Add(extraProvider);
            var allCustomers = new List<AppUser>();
            if (customer != null) allCustomers.Add(customer);
            if (extraCustomer != null) allCustomers.Add(extraCustomer);

            // Example context-aware service and comment templates
            var serviceTemplates = new Dictionary<string, (string title, string desc)[]> {
                ["تنظيف منازل"] = new[] {
                    ("تنظيف منازل شامل", "خدمة تنظيف منازل باحترافية عالية وتشمل جميع الغرف والأرضيات."),
                    ("تنظيف منازل سريع", "تنظيف منازل بسرعة وكفاءة مع مواد صديقة للبيئة.")
                },
                ["تنظيف سجاد"] = new[] {
                    ("تنظيف سجاد عميق", "خدمة تنظيف سجاد باستخدام أحدث الأجهزة لإزالة البقع والروائح."),
                    ("تنظيف سجاد بالبخار", "تنظيف سجاد بالبخار لتعقيم السجاد وجعله كالجديد.")
                },
                ["صيانة تكييف"] = new[] {
                    ("صيانة تكييف مركزي", "صيانة وتنظيف أجهزة التكييف المركزي بكفاءة عالية."),
                    ("صيانة تكييف سبليت", "خدمة صيانة تكييف سبليت مع ضمان على العمل.")
                },
                ["سباكة"] = new[] {
                    ("إصلاح تسربات المياه", "خدمة إصلاح تسربات المياه في الحمامات والمطابخ."),
                    ("تركيب مواسير جديدة", "تركيب مواسير مياه جديدة بجودة عالية.")
                },
                ["تركيب أثاث"] = new[] {
                    ("تركيب أثاث مكتبي", "تركيب جميع أنواع الأثاث المكتبي بسرعة ودقة."),
                    ("تركيب أثاث منزلي", "خدمة تركيب أثاث منزلي مع ضمان على التركيب.")
                },
                ["تركيب ستائر"] = new[] {
                    ("تركيب ستائر رول", "تركيب ستائر رول لجميع النوافذ.") ,
                    ("تركيب ستائر كلاسيك", "خدمة تركيب ستائر كلاسيك بتصاميم عصرية.")
                },
                ["إصلاح كهرباء"] = new[] {
                    ("إصلاح أعطال كهربائية", "إصلاح جميع الأعطال الكهربائية المنزلية بسرعة وأمان."),
                    ("تغيير مفاتيح كهرباء", "تغيير مفاتيح الكهرباء القديمة بجديدة.")
                },
                ["تركيب إضاءة"] = new[] {
                    ("تركيب إضاءة LED", "تركيب إضاءة LED موفرة للطاقة."),
                    ("تركيب ثريات", "خدمة تركيب ثريات وإضاءة زخرفية.")
                }
            };
            var commentTemplates = new Dictionary<string, string[]> {
                ["تنظيف"] = new[] { "الخدمة ممتازة والتنظيف رائع!", "تم التنظيف بسرعة وبدون مشاكل." },
                ["صيانة"] = new[] { "تمت الصيانة بكفاءة عالية.", "الفني كان محترفاً وسريعاً." },
                ["تركيب"] = new[] { "التركيب كان دقيقاً وسريعاً.", "خدمة رائعة في تركيب الأثاث." },
                ["كهرباء"] = new[] { "تم إصلاح الكهرباء بسرعة.", "الإضاءة الجديدة رائعة!" }
            };

            foreach (var subCat in subCategoryDict.Values)
            {
                // Find or create two services for this subcategory
                var subcatServices = await context.ServiceSubCategories
                    .Where(ssc => ssc.SubCategoryId == subCat.Id)
                    .Select(ssc => ssc.Service)
                    .ToListAsync();
                int toCreate = 2 - subcatServices.Count;
                var templates = serviceTemplates.ContainsKey(subCat.Name) ? serviceTemplates[subCat.Name] : new[] {
                    ($"خدمة {subCat.Name} إضافية 1", $"خدمة تلقائية للتصنيف الفرعي {subCat.Name}"),
                    ($"خدمة {subCat.Name} إضافية 2", $"خدمة تلقائية للتصنيف الفرعي {subCat.Name}")
                };
                for (int i = 0; i < toCreate; i++)
                {
                    var providerForService = allProviders[i % allProviders.Count];
                    var (title, desc) = templates[i % templates.Length];
                    var newService = new Models.Service
                    {
                        Title = title,
                        Description = desc,
                        EstimatedPrice = 20000m + 5000 * i,
                        ContactPhone = "0770000000" + i,
                        UserId = providerForService.Id,
                        Provider = providerForService,
                        IsApproved = true,
                        CreatedDate = DateTime.UtcNow.AddDays(-2 - i)
                    };
                    await context.Services.AddAsync(newService);
                    await context.SaveChangesAsync();
                    var ssc = new ServiceSubCategory
                    {
                        ServiceId = newService.Id,
                        Service = newService,
                        SubCategoryId = subCat.Id,
                        SubCategory = subCat
                    };
                    await context.ServiceSubCategories.AddAsync(ssc);
                    await context.SaveChangesAsync();
                    subcatServices.Add(newService);
                }
                // For each service in this subcategory, ensure it has at least two comments
                foreach (var svc in subcatServices)
                {
                    var svcComments = await context.Comments.Where(c => c.ServiceId == svc.Id).ToListAsync();
                    int commentsToCreate = 2 - svcComments.Count;
                    var catName = categoryDict.FirstOrDefault(x => x.Value.Id == subCat.CategoryId).Key ?? "";
                    var commentsArr = commentTemplates.ContainsKey(catName) ? commentTemplates[catName] : new[] { "خدمة جيدة.", "أنصح بها." };
                    for (int j = 0; j < commentsToCreate; j++)
                    {
                        var customerForComment = allCustomers[j % allCustomers.Count];
                        var commentText = commentsArr[j % commentsArr.Length] + $" ({customerForComment.FirstName})";
                        var comment = new Comment
                        {
                            Content = commentText,
                            UserId = customerForComment.Id,
                            User = customerForComment,
                            ServiceId = svc.Id,
                            Service = svc,
                            CreatedDate = DateTime.UtcNow.AddMinutes(-10 * (j + 1))
                        };
                        await context.Comments.AddAsync(comment);
                    }
                    await context.SaveChangesAsync();
                }
            }
            // --- END: Ensure each subcategory has at least two services and each service has at least two comments, with context-aware data ---
        }

        private static async Task EnsureRoleCreated(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private static async Task CreateUser(UserManager<AppUser> userManager, AppUser user, string password, string role)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
} 