# Wood Sales Platform - ASP.NET Core MVC

A complete e-commerce platform for wood products built with ASP.NET Core 8 MVC, Entity Framework Core, and SQL Server.

## ✅ All Issues Fixed

- ✅ All controllers properly routed
- ✅ All views connected to their endpoints
- ✅ Anti-forgery tokens added to all forms
- ✅ Proper error handling
- ✅ Success/Error notifications with TempData
- ✅ Image fallbacks for missing images
- ✅ Empty state handling
- ✅ Modern, responsive UI

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or SQL Server Express)

### Setup Steps

1. **Navigate to the project directory**:
   ```bash
   cd WoodSalesPlatform
   ```

2. **Update Connection String** (if needed):
   Edit `appsettings.json` and modify:
   ```json
   "DefaultConnection": "Server=localhost;Database=WoodSalesPlatformDB;Trusted_Connection=True;TrustServerCertificate=True;"
   ```

3. **Create Database**:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   
   **Note**: If you see a pending model changes warning, it's safe to ignore or you can remove and recreate migrations:
   ```bash
   dotnet ef migrations remove
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Run Application**:
   ```bash
   dotnet restore
   dotnet run
   ```

5. **Access Application**:
   - Open browser to `https://localhost:5001`
   - Admin login: **admin** / **admin123**

## 📋 Complete Endpoint-to-View Mapping

### Public Pages (Guest Access)

| Endpoint | Controller | Action | View | Description |
|----------|-----------|--------|------|-------------|
| `/` | HomeController | Index() | Views/Home/Index.cshtml | Homepage with categories |
| `/Home` | HomeController | Index() | Views/Home/Index.cshtml | Homepage (alternative) |
| `/Home/Index` | HomeController | Index() | Views/Home/Index.cshtml | Homepage (full path) |
| `/Category/Products/{id}` | CategoryController | Products(id) | Views/Category/Products.cshtml | Products in category |
| `/Product/Details/{id}` | ProductController | Details(id) | Views/Product/Details.cshtml | Product details page |
| `/Product/PlaceOrder` [POST] | ProductController | PlaceOrder(order) | Redirects to OrderSuccess | Place order |
| `/Product/OrderSuccess` | ProductController | OrderSuccess() | Views/Product/OrderSuccess.cshtml | Order confirmation |

### Admin Pages (Requires Authentication)

| Endpoint | Controller | Action | View | Description |
|----------|-----------|--------|------|-------------|
| `/Admin/Login` | AdminController | Login() [GET] | Views/Admin/Login.cshtml | Admin login page |
| `/Admin/Login` [POST] | AdminController | Login(username, password) | Redirects to Dashboard | Process login |
| `/Admin/Logout` | AdminController | Logout() | Redirects to Home | Logout admin |
| `/Admin/Dashboard` | AdminController | Dashboard() | Views/Admin/Dashboard.cshtml | Orders dashboard |
| `/Admin/ManageProducts` | AdminController | ManageProducts() | Views/Admin/ManageProducts.cshtml | Manage products |
| `/Admin/AddProduct` [POST] | AdminController | AddProduct(product) | Redirects to ManageProducts | Add new product |
| `/Admin/ToggleFeatured` [POST] | AdminController | ToggleFeatured(id) | Redirects to ManageProducts | Toggle featured status |
| `/Admin/DeleteProduct` [POST] | AdminController | DeleteProduct(id) | Redirects to ManageProducts | Delete product |
| `/Admin/ManageCategories` | AdminController | ManageCategories() | Views/Admin/ManageCategories.cshtml | Manage categories |
| `/Admin/AddCategory` [POST] | AdminController | AddCategory(category) | Redirects to ManageCategories | Add new category |
| `/Admin/DeleteCategory` [POST] | AdminController | DeleteCategory(id) | Redirects to ManageCategories | Delete category |

## 📁 Project Structure

```
WoodSalesPlatform/
├── Controllers/
│   ├── HomeController.cs          → Handles: / (Homepage)
│   ├── CategoryController.cs      → Handles: /Category/Products/{id}
│   ├── ProductController.cs       → Handles: /Product/Details/{id}, /Product/PlaceOrder
│   └── AdminController.cs         → Handles: All /Admin/* endpoints
├── Models/
│   ├── User.cs                    → Admin user model
│   ├── Category.cs                → Product category model
│   ├── Product.cs                 → Product model
│   └── Order.cs                   → Customer order model
├── Data/
│   └── ApplicationDbContext.cs    → EF Core database context
├── Services/
│   └── JwtService.cs              → JWT token generation
├── Views/
│   ├── Home/
│   │   └── Index.cshtml           → Homepage view
│   ├── Category/
│   │   └── Products.cshtml        → Category products view
│   ├── Product/
│   │   ├── Details.cshtml         → Product details view
│   │   └── OrderSuccess.cshtml    → Order confirmation view
│   ├── Admin/
│   │   ├── Login.cshtml           → Admin login view
│   │   ├── Dashboard.cshtml       → Orders dashboard view
│   │   ├── ManageProducts.cshtml  → Products management view
│   │   └── ManageCategories.cshtml → Categories management view
│   └── Shared/
│       └── _Layout.cshtml         → Main layout template
├── wwwroot/
│   └── css/
│       └── site.css               → All styles
├── Program.cs                     → App configuration
├── appsettings.json               → Configuration settings
└── WoodSalesPlatform.csproj       → Project file
```

## 🗄️ Database Models

### User
- Id (int, PK)
- Username (string)
- PasswordHash (string)
- IsAdmin (bool)

### Category
- Id (int, PK)
- Name (string)
- Description (string)
- ImageUrl (string) - Category image
- Products (navigation)

### Product
- Id (int, PK)
- Name (string)
- Description (string)
- Price (decimal)
- ImageUrl (string) - Main product image
- Stock (int)
- IsFeatured (bool) - Whether product appears in featured carousel
- CategoryId (int, FK)
- Category (navigation)
- ProductImages (navigation) - Additional product images

### ProductImage
- Id (int, PK)
- ImageUrl (string)
- IsPrimary (bool)
- ProductId (int, FK)
- Product (navigation)

### Order
- Id (int, PK)
- CustomerName (string)
- CustomerEmail (string)
- CustomerPhone (string)
- ShippingAddress (string)
- Quantity (int)
- TotalPrice (decimal)
- OrderDate (DateTime)
- ProductId (int, FK)
- Product (navigation)

## 🔐 Authentication

- **Type**: JWT-based cookie authentication
- **Default Admin**: admin / admin123
- **Cookie Name**: AuthToken
- **Expiry**: 24 hours
- **Protected Routes**: All /Admin/* endpoints except /Admin/Login

## 🎨 Features

### For Guests
- Browse categories
- **View featured products carousel** (shows up to 8 featured products with auto-slide)
- View products by category
- See product details
- Place orders with contact information
- Automatic stock deduction
- Order confirmation page

### For Admin
- Secure login with JWT
- View all orders with details
- **Upload product images from device** (main image + up to 5 additional images)
- **Upload category images from device**
- Add new products with images
- **Mark/unmark products as featured** (featured products appear in home carousel)
- Delete products
- Add new categories
- Delete categories
- Real-time statistics dashboard
- Image preview before upload

## 🛠️ Technologies

- **Backend**: ASP.NET Core 8 MVC
- **Database**: Entity Framework Core 8 + SQL Server
- **Authentication**: JWT Bearer Tokens
- **Password Hashing**: BCrypt.Net
- **Frontend**: Razor Views + Modern CSS
- **Fonts**: Syne (headings), DM Sans (body)

## 📝 Usage Guide

### Adding Your First Category
1. Login as admin (`/Admin/Login`)
2. Go to "Categories" in navigation
3. Fill form: Name, Description
4. **Upload category image** from your device
5. Click "Add Category"

### Adding Products
1. Go to "Products" in navigation
2. Select a category from dropdown
3. Fill product details (name, description, price, stock)
4. **Upload main product image** from your device
5. **Optionally upload up to 5 additional images** for product gallery
6. See image preview before submitting
7. Click "Add Product"

### Managing Orders
1. Go to "Dashboard" to see all orders
2. View customer details, shipping address
3. Check order totals and dates

### Managing Featured Products
1. Go to "Products" in navigation
2. Find the product you want to feature
3. Click "Make Featured" button (up to 8 products can be featured)
4. Featured products will appear in the carousel on the home page
5. Click "Remove Featured" to unfeature a product
6. Carousel auto-rotates every 5 seconds with navigation arrows

## 🔧 Configuration

### JWT Settings (appsettings.json)
```json
"Jwt": {
  "Key": "YourSuperSecretKeyForJWTTokenGeneration...",
  "Issuer": "WoodSalesPlatform",
  "Audience": "WoodSalesPlatformUsers"
}
```

### Database Connection
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=WoodSalesPlatformDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

## 📱 Responsive Design

- Mobile-friendly navigation
- Responsive grids
- Touch-optimized buttons
- Flexible layouts

## ⚠️ Important Notes

1. **Anti-Forgery Tokens**: All POST forms include `@Html.AntiForgeryToken()` for CSRF protection
2. **Image URLs**: Use valid image URLs or placeholders will be shown
3. **Stock Management**: Orders automatically reduce product stock
4. **Authentication**: Cookie-based, stored securely with HttpOnly flag
5. **Error Handling**: All operations include try-catch and user feedback

## 🐛 Troubleshooting

### Database Issues
```bash
# Reset database
dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Login Issues
- Check database for admin user
- Verify password is "admin123"
- Clear browser cookies

### View Not Found
- Check controller name matches folder name
- Verify action name matches view file name
- Ensure _ViewStart.cshtml exists

## 📄 License

This project is provided as-is for educational and commercial use.

## 👨‍💻 Support

For issues or questions:
1. Check endpoint mappings above
2. Verify database connection
3. Check browser console for errors
4. Review controller routing
"# SimpleSale-Platform" 
