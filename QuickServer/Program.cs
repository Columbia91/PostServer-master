using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuickServer
{
    class Program
    {
        private static Socket sListener;

        static void Main(string[] args)
        {
            try
            {
                // Устанавливаем для сокета локальную конечную точку
                IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

                // Создаем сокет Tcp/Ip
                sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                //WorkWithClients(sListener);
                sListener.BeginAccept(WorkWithClients, null);
                Console.ReadLine();
                sListener.Shutdown(SocketShutdown.Both);
                sListener.Close();
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void WorkWithClients(object state)
        {
            try
            {
                // Начинаем слушать соединения
                while (true)
                {
                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.EndAccept(state as IAsyncResult);
                    sListener.BeginAccept(WorkWithClients, null);
                    string index = null;

                    // Мы дождались клиента, пытающегося с нами соединиться
                    Console.WriteLine("\nНовое соединение с клиентом");

                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    index += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    // Показываем данные на консоли
                    Console.WriteLine("Сервер получил запрос на обработку...");

                    // Отправляем ответ клиенту\
                    List<string> reply = GetStreetsFromIndex(index);

                    byte[] msg = reply.SelectMany(r => r)
                                      .Select(Convert.ToByte)
                                      .ToArray();

                    handler.Send(msg);

                    Console.WriteLine("Сервер отправил ответ клиенту\n" +
                        "Завершение соединения с клиентом.");

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static List<string> GetStreetsFromIndex(string index)
        {
            List<string> response = new List<string>();
            try
            {
                using (PostContext context = new PostContext())
                {
                    response = context.PostalRecords.Where(p => p.Index == index)
                                                    .Select(p => p.Street).ToList();
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
    }
}
