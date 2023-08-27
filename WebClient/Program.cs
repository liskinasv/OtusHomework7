using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebClient
{
    static class Program
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        static async Task Main()
        {
            while (true)
            {

                #region GET-запрос на получение пользователя по идентификатору

                Console.WriteLine("Введите Id покупателя:");

                long customerId = Convert.ToInt64(Console.ReadLine());

                var response = await _httpClient.GetAsync($"https://localhost:7256/api/Customers/{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    var customer = await response.Content.ReadFromJsonAsync<Customer>();
                    Console.WriteLine($"Покупатель: Id:{customer.Id} {customer.Firstname} {customer.Lastname}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Покупатель не найден");
                }
                else
                {
                    Console.WriteLine("Ошибка получения покупателя.");
                }
                #endregion


                Console.WriteLine();


                #region POST-запрос на добавление пользователя

                Console.WriteLine("Добавить нового покупателя?  y - да, n - нет, q - выход из программы ");

                string answer = Console.ReadLine();

                if (answer == "y")
                {

                    var customer = GegerateRandomCustomer();

                    //debug
                    //customer = new Customer { Id = 1, Firstname = "Мартин", Lastname = "Фаулер" };


                    response = await _httpClient.PostAsJsonAsync("https://localhost:7256/api/Customers", customer);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Покупатель: Id:{customer.Id} {customer.Firstname} {customer.Lastname} добавлен успешно.");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        Console.WriteLine($"Покупатель с Id = {customer.Id} уже существует.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка добавления покупателя.");
                    }
                }
                else if (answer == "q")
                {
                    break;
                }

                Console.WriteLine();
                #endregion

            }
        }




        private static Customer GegerateRandomCustomer()
        {
            var random = new Random();
            int id = random.Next();
            var customer = new Customer
            {
                Id = id,
                Firstname = $"FirstName{id}",
                Lastname = $"LastName{id}"
            };
            return customer;
        }

    }
}