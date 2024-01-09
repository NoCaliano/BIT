using BIT.Models;

namespace BIT.FirstSetup
{
    public class FirstData
    {
        public List<Dish> fdish = new List<Dish>();
        public List<Category> fcat = new List<Category>();

        public FirstData()
        {
            // Adding Pizza dishes
            Dish dish1 = new Dish()
            {
                Name = "CHEESEEPIZZA",
                Img = "https://thumb.tildacdn.com/tild6564-3633-4636-b761-656465306433/-/resize/622x/-/format/webp/Cheesee_pizza.png",
                Description = "Good stuff",
                Price = 189,
                Calories = 343,
                IsAvaileble = true,
                IsFavorite = true,
                Category = "Pizza"
            };

            Dish dish2 = new Dish()
            {
                Name = "ГАВАЙСЬКА",
                Img = "https://thumb.tildacdn.com/tild6465-3532-4064-b137-663635393639/-/resize/622x/-/format/webp/photo.png",
                Description = "Good stuff",
                Price = 169,
                Calories = 343,
                IsAvaileble = true,
                IsFavorite = true,
                Category = "Pizza"
            };

            Dish dish3 = new Dish()
            {
                Name = "ЦЕЗАР",
                Img = "https://thumb.tildacdn.com/tild6465-3532-4064-b137-663635393639/-/resize/622x/-/format/webp/photo.png",
                Description = "Tasty",
                Price = 229,
                Calories = 343,
                IsAvaileble = true,
                IsFavorite = true,
                Category = "Pizza"
            };

            Dish dish4 = new Dish()
            {
                Name = "АЛЯСКА",
                Img = "https://thumb.tildacdn.com/tild6536-3431-4337-a563-376530393063/-/resize/622x/-/format/webp/photo.png",
                Description = "соус томатний, моцарелла, бекон, огірок маринований, шампіньйони, цибуля синя, сосиски баварські",
                Price = 179,
                Calories = 343,
                IsAvaileble = true,
                IsFavorite = true,
                Category = "Pizza"
            };

            // Adding Cakes dishes
            Dish dish5 = new Dish()
            {
                Name = "ТІСТЕЧКО «ЕДЕМ»",
                Img = "https://vatsak.com.ua/image/cache/catalog/products/Tistechka/Premiera/Premiera_Icon-562x429.png",
                Description = "Улюблений усіма класичний десерт у авторському виконанні – смак молочного суфле з легкою, повітряною консистенцією, вкритий тонким шаром шоколадної глазурі. Десерт викладено на подушку насичненого шоколадного бісквіту, що робить його смак ще яскравішим.",
                Price = 125,
                Calories = 343,
                IsAvaileble = true,
                IsFavorite = true,
                Category = "Cakes"
            };

            Dish dish6 = new Dish()
            {
                Name = "ТІСТЕЧКО «LOVE»",
                Img = "https://vatsak.com.ua/image/cache/catalog/products/Tistechka/Love/Love_Icon2-562x429.png",
                Description = "Тістечко, що закохує з першого погляду та назавжди залишається у серці після першого шматочку – це легке крем-суфле, прошаркок ягідної полуничної начинки, та яскравий шар полуничного мармеладу, викладені на подушку світлого пористого бісквіту.",
                Price = 149,
                Calories = 343,
                IsAvaileble = true,
                IsFavorite = true,
                Category = "Cakes"
            };

            Dish dish7 = new Dish()
            {
                Name = "ТІСТЕЧКО «МАЛІБУ»",
                Img = "https://vatsak.com.ua/image/cache/catalog/products/Tistechka/Malibu/Malibu_Icon-562x429.png",
                Description = "Яскравий смак Банан-Шоколад. Банановий вершковий крем із фруктовим наповнювачем з цільними шматочками банану, викладений на подушку насиченого шоколадного бісквіту.",
                Price = 149,
                Calories = 343,
                IsAvaileble = true,
                IsFavorite = true,
                Category = "Cakes"
            };

            // Adding Sushi dishes
            Dish dish8 = new Dish()
            {
                Name = "ТІСТЕЧКО «МАЛІБУ»",
                Img = "https://frankivskyygroup.com.ua/wp-content/uploads/2022/03/Vulkan-scaled.jpg",
                Description = "Філе лосося,лосось в соусі теріякі,крем-сир,огірок,ікра масаго,соус спайсі,соус унагі,зелена цибуля\r\n",
                Price = 369,
                Calories = 343,
                IsAvaileble = true,
                IsFavorite = true,
                Category = "Sushi"
            };

            Dish dish9 = new Dish()
            {
                Name = "Еверест",
                Img = "https://frankivskyygroup.com.ua/wp-content/uploads/2023/05/Everest-768x768.jpg",
                Description = "Вугор,крем-сир,омлет тамаго,авокадо,ікра масаго\r\n",
                Price = 449,
                Calories = 343,
                IsAvaileble = true,
                IsFavorite = true,
                Category = "Sushi"
            };

            Dish dish10 = new Dish()
            {
                Name = "Леопард",
                Img = "https://frankivskyygroup.com.ua/wp-content/webp-express/webp-images/uploads/2023/05/Leopard-768x768.jpg.webp",
                Description = "Лосось в соусі теріякі,крем сир,огірок,зелена цибуля,чедер,соус унагі,кунжут білий\r\n",
                Price = 319,
                Calories = 343,
                IsAvaileble = true,
                IsFavorite = true,
                Category = "Sushi"
            };

            // Adding Categories
            Category Pizza = new Category()
            {
                Name = "Pizza",
                Description = "Мука і всьо таке"
            };

            Category Cakes = new Category()
            {
                Name = "Cakes",
                Description = "Мука і всьо таке"
            };

            Category Sushi = new Category()
            {
                Name = "Sushi",
                Description = "Риба і всьо таке"
            };

            // Adding dishes and categories to lists
            fdish.AddRange(new List<Dish> { dish1, dish2, dish3, dish4, dish5, dish6, dish7, dish8, dish9, dish10 });
            fcat.AddRange(new List<Category> { Pizza, Cakes, Sushi });
        }
    }
}

