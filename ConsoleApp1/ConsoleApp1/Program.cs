using System;

namespace TextRPG
{
    internal class Program
    {
        static Player player = new Player("플레이어", "모험가"); // 기본 캐릭터 생성
        static Shop shop = new Shop(); // 상점 생성

        static void Main(string[] args)
        {
            StartGame(); // 게임 시작
        }

        static void StartGame()
        {
            bool gameRunning = true; // 게임이 실행 중인지 확인하는 플래그

            while (gameRunning)
            {
                Console.Clear(); // 화면 지우기
                Console.WriteLine("\n==== 텍스트 RPG ====");
                Console.WriteLine("1. 던전");
                Console.WriteLine("2. 상태 보기");
                Console.WriteLine("3. 인벤토리");
                Console.WriteLine("4. 상점");
                Console.WriteLine("0. 게임 종료");
                Console.Write("\n선택지를 입력하세요: ");

                string choice = Console.ReadLine() ?? string.Empty; // 사용자 입력 받기

                switch (choice)
                {
                    case "1":
                        EnterDungeon();
                        break;
                    case "2":
                        ShowStatus();
                        break;
                    case "3":
                        ManageInventory();
                        break;
                    case "4":
                        VisitShop();
                        break;
                    case "0":
                        Console.WriteLine("게임을 종료합니다.");
                        gameRunning = false;
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                        break;
                }
            }
        }

        // 던전 탐험 선택지
        static void EnterDungeon()
        {
            Console.Clear();
            Console.WriteLine("던전에 들어갔습니다! 아직 구현 중입니다.");
            Console.WriteLine("\n계속하려면 Enter 키를 누르세요...");
            Console.ReadLine();
        }

        // 상태 보기 선택지
        static void ShowStatus()
        {
            Console.Clear();
            Console.WriteLine("==== [ 캐릭터 상태 ] ====");
            player.DisplayStatus(); // 캐릭터 상태 출력
            Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
            Console.ReadKey(); // 상태 보기에서만 사용
        }

        // 인벤토리 관리 선택지
        // 인벤토리 관리 선택지
        // 인벤토리 관리 선택지
        static void ManageInventory()
        {
            bool inInventory = true; // 인벤토리 관리 중인지 확인하는 플래그

            while (inInventory)
            {
                Console.Clear();
                Console.WriteLine("==== [ 인벤토리 ] ====");

                // 인벤토리가 비었는지 확인
                if (player.InventoryCount == 0)
                {
                    Console.WriteLine("아무것도 없는 상태입니다.");
                    Console.WriteLine("0. 나가기");
                    Console.Write("\n선택지를 입력하세요: ");
                    string input = Console.ReadLine() ?? string.Empty;

                    // 0을 입력하면 인벤토리 종료
                    if (input == "0")
                    {
                        inInventory = false;
                    }
                }
                else
                {
                    // 인벤토리에 있는 아이템 출력 (장착된 아이템은 [E] 표시)
                    for (int i = 0; i < player.InventoryCount; i++)
                    {
                        string equippedIndicator = player.Inventory[i].IsEquipped ? "[E] " : "";
                        Console.WriteLine($"{i + 1}. {equippedIndicator}{player.Inventory[i].Name} - {player.Inventory[i].Description}");
                    }

                    Console.WriteLine("\n장착할 아이템 번호를 입력하세요 (0. 나가기): ");
                    string input = Console.ReadLine() ?? string.Empty;

                    if (int.TryParse(input, out int itemNumber) && itemNumber > 0 && itemNumber <= player.InventoryCount)
                    {
                        Item selectedItem = player.Inventory[itemNumber - 1];

                        if (selectedItem.Type == ItemType.Weapon || selectedItem.Type == ItemType.Armor)
                        {
                            // 이미 장착된 아이템이 있는 경우 해제
                            if (selectedItem.IsEquipped)
                            {
                                player.UnequipItem(selectedItem); // 장비 해제 함수 호출
                                Console.WriteLine($"{selectedItem.Name}을(를) 해제했습니다.");
                            }
                            else
                            {
                                player.EquipItem(selectedItem); // 장비 장착 함수 호출
                                Console.WriteLine($"{selectedItem.Name}을(를) 장착했습니다.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("이 아이템은 장착할 수 없습니다.");
                        }
                    }
                    else if (itemNumber == 0)
                    {
                        Console.WriteLine("인벤토리에서 나갑니다.");
                        inInventory = false; // 인벤토리 종료
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }
        }



        // 상점 방문 선택지
        static void VisitShop()
        {
            shop.ShowShop(player); // 상점 출력
        }
    }

    // 아이템 클래스
    class Item
    {
        public string Name { get; set; } // 아이템 이름
        public string Description { get; set; } // 아이템 설명
        public bool IsPurchased { get; set; } = false; // 구매 여부
        public bool IsEquipped { get; set; } = false; // 장착 여부
        public int Price { get; set; } // 아이템 가격
        public string Effect { get; set; } // 아이템 효과 (공격력/방어력 +)
        public ItemType Type { get; set; } // 아이템 타입 (무기, 방어구 등)

        // 생성자
        public Item(string name, string effect, string description, int price, ItemType type)
        {
            Name = name;
            Effect = effect;
            Description = description;
            Price = price;
            Type = type;
        }

        // 아이템 정보 출력 (구매 여부 확인)
        public override string ToString()
        {
            return IsPurchased ? $"{Name} | {Effect} | {Description} | 구매완료" : $"{Name} | {Effect} | {Description} | {Price} G";
        }
    }

    // 플레이어 클래스
    class Player
    {
        public string Name { get; set; } // 이름
        public string Job { get; set; }  // 직업
        public int Level { get; set; } = 1; // 기본 레벨
        public int Attack { get; set; } = 10; // 기본 공격력
        public int Defense { get; set; } = 5; // 기본 방어력
        public int MaxHealth { get; set; } = 100; // 최대 체력
        public int CurrentHealth { get; set; } = 100; // 현재 체력
        public int Gold { get; set; } = 8000; // 기본 소지금

        public Item? EquippedWeapon { get; set; } // 장착된 무기
        public Item? EquippedArmor { get; set; }  // 장착된 방어구

        public Item[] Inventory { get; private set; } // 아이템 배열
        public int InventoryCount { get; private set; } = 0; // 인벤토리 내 아이템 수

        // 생성자
        public Player(string name, string job)
        {
            Name = name;
            Job = job;
            Inventory = new Item[10]; // 최대 10개의 아이템 보유 가능
        }

        // 인벤토리에 아이템 추가
        public void AddItem(Item item)
        {
            if (InventoryCount < Inventory.Length)
            {
                Inventory[InventoryCount] = item;
                InventoryCount++;
            }
            else
            {
                Console.WriteLine("인벤토리가 가득 찼습니다.");
            }
        }

        // 장비 장착 함수
        public void EquipItem(Item item)
        {
            if (item.Type == ItemType.Weapon)
            {
                // 기존 무기 해제
                if (EquippedWeapon != null)
                {
                    EquippedWeapon.IsEquipped = false;
                    Attack -= GetItemEffect(EquippedWeapon); // 이전 무기의 효과 제거
                    Console.WriteLine($"{EquippedWeapon.Name}을(를) 해제했습니다.");
                }

                // 새로운 무기 장착
                EquippedWeapon = item;
                item.IsEquipped = true;
                Attack += GetItemEffect(item); // 새로운 무기의 효과 추가
            }
            else if (item.Type == ItemType.Armor)
            {
                // 기존 방어구 해제
                if (EquippedArmor != null)
                {
                    EquippedArmor.IsEquipped = false;
                    Defense -= GetItemEffect(EquippedArmor); // 이전 방어구의 효과 제거
                    Console.WriteLine($"{EquippedArmor.Name}을(를) 해제했습니다.");
                }

                // 새로운 방어구 장착
                EquippedArmor = item;
                item.IsEquipped = true;
                Defense += GetItemEffect(item); // 새로운 방어구의 효과 추가
            }
        }

        // 아이템의 효과를 반환하는 메서드
        private int GetItemEffect(Item item)
        {
            if (item.Effect.StartsWith("공격력 +"))
            {
                return int.Parse(item.Effect.Split('+')[1]);
            }
            else if (item.Effect.StartsWith("방어력 +"))
            {
                return int.Parse(item.Effect.Split('+')[1]);
            }
            return 0;
        }

        // 장비 해제 함수
        public void UnequipItem(Item item)
        {
            if (item.IsEquipped)
            {
                if (item == EquippedWeapon)
                {
                    Attack -= GetItemEffect(item); // 장비 해제 시 효과 감소
                    EquippedWeapon = null;
                }
                else if (item == EquippedArmor)
                {
                    Defense -= GetItemEffect(item); // 장비 해제 시 효과 감소
                    EquippedArmor = null;
                }

                item.IsEquipped = false; // 장비 해제 상태로 변경
                Console.WriteLine($"{item.Name}을(를) 해제했습니다.");
            }
        }

        // 캐릭터 상태 출력
        public void DisplayStatus()
        {
            Console.WriteLine($"이름       : {Name}");
            Console.WriteLine($"직업       : {Job}");
            Console.WriteLine($"레벨       : {Level}");
            Console.WriteLine($"공격력     : {Attack}");
            Console.WriteLine($"방어력     : {Defense}");
            Console.WriteLine($"체력       : {CurrentHealth}/{MaxHealth}");
            Console.WriteLine($"Gold       : {Gold}");
        }

        // 인벤토리에서 아이템 제거 함수
        public void RemoveItem(Item item)
        {
            for (int i = 0; i < InventoryCount; i++)
            {
                if (Inventory[i] == item)
                {
                    // 아이템을 인벤토리에서 제거
                    for (int j = i; j < InventoryCount - 1; j++)
                    {
                        Inventory[j] = Inventory[j + 1];
                    }
                    InventoryCount--;
                    break;
                }
            }
        }
    }

    // 상점 클래스
    class Shop
    {
        private Item[] shopItems; // 상점에서 판매하는 아이템 목록

        // 생성자
        public Shop()
        {
            shopItems = new Item[]
            {
                new Item("수련자 갑옷", "방어력 +5", "수련에 도움을 주는 갑옷입니다.", 1000, ItemType.Armor),
                new Item("무쇠갑옷", "방어력 +9", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1500, ItemType.Armor),
                new Item("스파르타의 갑옷", "방어력 +15", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, ItemType.Armor),
                new Item("낡은 검", "공격력 +2", "쉽게 볼 수 있는 낡은 검 입니다.", 600, ItemType.Weapon),
                new Item("청동 도끼", "공격력 +5", "어디선가 사용됐던 거 같은 도끼입니다.", 1500, ItemType.Weapon),
                new Item("스파르타의 창", "공격력 +7", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000, ItemType.Weapon)
            };
        }

        // 상점 출력
        public void ShowShop(Player player)
        {
            bool inShop = true;
            while (inShop)
            {
                Console.Clear();
                Console.WriteLine("주인: 어서오세요. 상점24 입니다.");
                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요: ");
                string choice = Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        BuyItem(player);  // 아이템 구매 기능으로 이동
                        break;
                    case "2":
                        SellItem(player);  // 아이템 판매 기능으로 이동
                        break;
                    case "0":
                        inShop = false;
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }

        // 아이템 구매 기능
        public void BuyItem(Player player)
        {
            Console.Clear();
            bool buying = true; // 계속 구매 여부를 위한 플래그
            while (buying)
            {
                Console.WriteLine($"\n[보유 골드]\n{player.Gold} G\n");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < shopItems.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {shopItems[i]}");
                }
                Console.Write("\n구매할 아이템 번호를 입력하세요 (0.나가기): ");
                if (int.TryParse(Console.ReadLine(), out int itemNumber) && itemNumber > 0 && itemNumber <= shopItems.Length)
                {
                    Item selectedItem = shopItems[itemNumber - 1];

                    if (selectedItem.IsPurchased)
                    {
                        Console.Clear();
                        Console.WriteLine("이미 구매한 아이템입니다.");
                    }
                    else if (player.Gold >= selectedItem.Price)
                    {
                        // 아이템 구매 처리
                        player.Gold -= selectedItem.Price;
                        selectedItem.IsPurchased = true;
                        player.AddItem(selectedItem);
                        Console.Clear();
                        Console.WriteLine("구매를 완료했습니다.");
                        buying = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Gold가 부족합니다.");
                        buying = true;
                    }
                }
                else if (itemNumber == 0)
                {
                    buying = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    buying = true;
                }
            }
        }

        // 아이템 판매 기능
        public void SellItem(Player player)
        {
            Console.Clear();
            Console.WriteLine("==== [ 판매 가능한 아이템 목록 ] ====");

            // 인벤토리에 판매 가능한 아이템이 있는지 확인
            if (player.InventoryCount == 0)
            {
                Console.WriteLine("판매할 아이템이 없습니다.");
            }
            else
            {
                // 인벤토리의 아이템 출력 (장착된 아이템은 [E] 표시)
                for (int i = 0; i < player.InventoryCount; i++)
                {
                    string equippedIndicator = player.Inventory[i].IsEquipped ? "[E] " : "";
                    Console.WriteLine($"{i + 1}. {equippedIndicator}{player.Inventory[i].Name} - {player.Inventory[i].Description}");
                }

                Console.Write("\n판매할 아이템 번호를 입력하세요 (0. 나가기): ");
                string input = Console.ReadLine() ?? string.Empty; ;
                if (int.TryParse(input, out int itemNumber) && itemNumber > 0 && itemNumber <= player.InventoryCount)
                {
                    Item selectedItem = player.Inventory[itemNumber - 1];

                    // 장착된 아이템 해제
                    if (selectedItem.IsEquipped)
                    {
                        player.UnequipItem(selectedItem);
                    }

                    // 판매 금액 계산 (85% 가격)
                    int sellPrice = (int)(selectedItem.Price * 0.85);
                    player.Gold += sellPrice;

                    // 인벤토리에서 아이템 제거
                    player.RemoveItem(selectedItem);

                    Console.WriteLine($"{selectedItem.Name}을(를) {sellPrice} G에 판매했습니다.");
                }
                else if (itemNumber == 0)
                {
                    Console.WriteLine("판매를 취소했습니다.");
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }

    // 아이템 유형 열거형 추가
    enum ItemType
    {
        Weapon,
        Armor
    }
}