HOTEL ROOM RESERVATION SYSTEM
=============================

🧾 Описание:
Конзолно приложение на C# за управление на хотелски резервации с използване на JSON файлове за съхранение на данни. 
Проектът поддържа потребителски профили, резервации, анулации, администраторски панел и резервни копия (backup).

📁 Структура на проекта:
├── Program.cs
├── Models/
│   ├── User.cs
│   ├── Room.cs
│   ├── Booking.cs
│   └── RoomType.cs
├── Services/
│   ├── UserService.cs
│   ├── RoomService.cs
│   ├── BookingService.cs
│   └── AdminService.cs
├── Utils/
│   ├── FileHelper.cs
│   └── BackupService.cs
├── data/
│   ├── users.json
│   ├── rooms.json
│   ├── bookings.json
│   └── room_types.json

⚙️ Изисквания:
- .NET 6 или по-нова версия
- Visual Studio или друга съвместима IDE

▶️ Стартиране:
1. Отвори проекта в Visual Studio.
2. Стартирай `Program.cs`.
3. Използвай числовото меню в конзолата за навигация.

👤 Потребителски функционалности:
1. Register – регистрация на нов потребител
2. Login – вход с име и парола
3. View Rooms – списък със свободни стаи
4. Book a Room – създаване на резервация
5. Cancel Booking – отказване на резервация по ID
6. View My Profile – показва потребителски резервации

🔐 Администраторски панел (Admin Mode):
- Достъп с парола: `admin123`
- View All Bookings – всички резервации
- View Total Income – приходи от потвърдени резервации
- View Cancellation Fees – общи такси при анулации
- Add / Remove Room – управление на стаи
- Backup All Data – създаване на резервни копия в папка `/backup`
- Search Bookings – по потребител, номер на стая или дати

📦 JSON файлове:
- `users.json` – съдържа регистрирани потребители
- `rooms.json` – налични хотелски стаи и статуси
- `bookings.json` – направени резервации
- `room_types.json` – типове стаи с екстри и капацитет

🔁 Backup:
При избор от админ панела, се създава папка `backup/yyyyMMdd_HHmmss/` с копие на всички JSON файлове.

📍 Забележка:
Увери се, че всички JSON файлове са в папка `data/`, както е зададено в кода.

👨‍💻 Автор: Л.А. (L.A.)
