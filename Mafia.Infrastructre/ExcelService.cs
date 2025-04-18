using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using OfficeOpenXml;

namespace Mafia.Infrastructre
{
    public class ExcelService : IExcelService
    {
        private readonly string _excelFolderPath;
        private readonly string _excelFilePath;

        public ExcelService()
        {
            // Настраиваем лицензию для EPPlus (необходима для коммерческого использования)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            // Создаем каталог для файлов Excel, если он не существует
            _excelFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DeliveredOrders");
            if (!Directory.Exists(_excelFolderPath))
            {
                Directory.CreateDirectory(_excelFolderPath);
            }
            
            // Путь к файлу Excel
            _excelFilePath = Path.Combine(_excelFolderPath, "DeliveredOrders.xlsx");
        }

        public async Task SaveDeliveredOrderToExcel(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            if (order.Status != OrderStatusEnum.Delivered)
            {
                throw new InvalidOperationException("Only delivered orders can be saved to Excel");
            }

            FileInfo fileInfo = new FileInfo(_excelFilePath);
            
            using (var package = new ExcelPackage(fileInfo))
            {
                // Получаем или создаем лист "Заказы"
                var worksheet = package.Workbook.Worksheets["Заказы"] ?? package.Workbook.Worksheets.Add("Заказы");
                
                // Если лист новый, добавляем заголовки
                if (worksheet.Dimension == null)
                {
                    worksheet.Cells[1, 1].Value = "ID заказа";
                    worksheet.Cells[1, 2].Value = "ID пользователя";
                    worksheet.Cells[1, 3].Value = "Дата заказа";
                    worksheet.Cells[1, 4].Value = "Сумма заказа";
                    worksheet.Cells[1, 5].Value = "Статус";
                    worksheet.Cells[1, 6].Value = "Адрес";
                    worksheet.Cells[1, 7].Value = "Способ оплаты";
                    worksheet.Cells[1, 8].Value = "Товары";
                    
                    // Стиль заголовков
                    using (var range = worksheet.Cells[1, 1, 1, 8])
                    {
                        range.Style.Font.Bold = true;
                    }
                }

                // Определяем следующую строку для данных
                int row = (worksheet.Dimension?.Rows ?? 0) + 1;
                
                // Заполняем данные заказа
                worksheet.Cells[row, 1].Value = order.Id;
                worksheet.Cells[row, 2].Value = order.UserId;
                worksheet.Cells[row, 3].Value = order.OrderDate.ToString("dd.MM.yyyy HH:mm:ss");
                worksheet.Cells[row, 4].Value = order.TotalAmount;
                worksheet.Cells[row, 5].Value = order.Status.ToString();
                worksheet.Cells[row, 6].Value = order.Address;
                worksheet.Cells[row, 7].Value = order.PaymentMethod.ToString();
                
                // Заполняем данные о товарах
                string orderDetails = string.Empty;
                if (order.OrderDetails != null && order.OrderDetails.Any())
                {
                    orderDetails = string.Join(", ", order.OrderDetails.Select(od => 
                        $"{(od.Product?.Name ?? "Неизвестный товар")} - {od.Quantity} шт. x {od.Price} руб."));
                }
                worksheet.Cells[row, 8].Value = orderDetails;
                
                // Автоматическое изменение ширины столбцов
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                
                // Сохраняем файл
                await package.SaveAsync();
            }
        }
    }
} 