using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartGame();
        }

        static void StartGame()
        {
            bool gameRunning = true;

            while (gameRunning)
            {
                Console.Clear();
                Console.WriteLine("\n==== Text RPG ====");
                Console.WriteLine("1. 스테이터스");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점 가기");
                Console.WriteLine("4. 종료");
                Console.Write("원하는 행동을 선택하세요): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowStatus();
                        break;
                    case "2":
                        ManageInventory();
                        break;
                    case "3":
                        VisitShop();
                        break;
                    case "4":
                        Console.WriteLine("게임을 종료합니다.");
                        gameRunning = false;
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

                Console.WriteLine();
            }
        }

        /*-----------------------------------------------상태보기-------------------------------------------------------------*/
        class Player
        {
            public string Name { get; set; }
            public string Job { get; set; }
            public int Level { get; set; } = 1;
            public float Attack { get; set; } = 10;
            public float Defense { get; set; } = 5;
            public int Health { get; set; } = 100;
            public int MaxHealth { get; set; } = 100;
            public int Gold { get; set; } = 300;

            public Item EquippedWeapon { get; set; }
            public Item EquippedArmor { get; set; }
            public Item EquippedAccessory { get; set; } // 보조장비 슬롯 추가

            public void ShowStatus()
            {
                Console.WriteLine($"이름 : {Name}");
                Console.WriteLine($"직업 : {Job}");
                Console.WriteLine($"레벨 : {Level}");
                Console.WriteLine($"공격력 : {Attack}");
                Console.WriteLine($"방어력 : {Defense}");
                Console.WriteLine($"체력 : {Health}/{MaxHealth}");
                Console.WriteLine($"Gold : {Gold}");
                Console.WriteLine($"무기 : {EquippedWeapon?.Name ?? "없음"}");
                Console.WriteLine($"방어구 : {EquippedArmor?.Name ?? "없음"}");
                Console.WriteLine($"보조장비: {EquippedAccessory?.Name ?? "없음"}"); // 보조장비 표시
            }

            public void EquipItem(Item item)
            {
                if (item.Type == ItemType.Weapon)
                {
                    if (EquippedWeapon != null)
                    {
                        UnequipItem(EquippedWeapon); // 기존 무기 해제
                    }

                    // 무기 장착
                    if (item.WeaponType == WeaponType.TwoHanded && EquippedAccessory != null)
                    {
                        UnequipItem(EquippedAccessory); // 양손검일 경우 보조장비 해제
                    }

                    EquippedWeapon = item;
                    Attack += item.BonusAttack;
                    Console.WriteLine($"{item.Name}을(를) 장착했습니다. (무기)");
                }
                else if (item.Type == ItemType.Armor)
                {
                    if (EquippedArmor != null)
                    {
                        UnequipItem(EquippedArmor); // 기존 방어구 해제
                    }
                    EquippedArmor = item;
                    Defense += item.BonusDefense;
                    MaxHealth += item.BonusHealth;
                    Console.WriteLine($"{item.Name}을(를) 장착했습니다. (방어구)");
                }
                else if (item.Type == ItemType.Accessory)
                {
                    // 보조장비는 한손검일 때만 가능
                    if (EquippedWeapon?.WeaponType == WeaponType.TwoHanded)
                    {
                        Console.WriteLine("양손검 장착 중에는 보조장비를 장착할 수 없습니다. 무기가 해제됩니다.");
                        UnequipItem(EquippedWeapon); // 양손검 해제
                    }

                    if (EquippedAccessory != null)
                    {
                        UnequipItem(EquippedAccessory); // 기존 보조장비 해제
                    }
                    EquippedAccessory = item;
                    Defense += item.BonusDefense; // 보조장비는 방어력 증가
                    Console.WriteLine($"{item.Name}을(를) 장착했습니다. (보조장비)");
                }

                item.IsEquipped = true;
            }

            public void UnequipItem(Item item)
            {
                if (item.IsEquipped)
                {
                    if (item.Type == ItemType.Weapon)
                    {
                        Attack -= item.BonusAttack;
                        EquippedWeapon = null;
                    }
                    else if (item.Type == ItemType.Armor)
                    {
                        Defense -= item.BonusDefense;
                        MaxHealth -= item.BonusHealth;
                        if (Health > MaxHealth) Health = MaxHealth;
                        EquippedArmor = null;
                    }
                    else if (item.Type == ItemType.Accessory)
                    {
                        Defense -= item.BonusDefense;
                        EquippedAccessory = null;
                    }

                    item.IsEquipped = false;
                    Console.WriteLine($"{item.Name}을(를) 해제했습니다.");
                }
            }
        }

        static Player player = new Player { Name = "주인공", Job = "사원" };

        static void ShowStatus()
        {
            Console.Clear();

            Console.WriteLine("==== [ 스테이터스 ] ====");
            Console.WriteLine($"이름   : {player.Name}");
            Console.WriteLine($"직업   : {player.Job}");
            Console.WriteLine($"레벨   : {player.Level}");
            Console.WriteLine($"공격력 : {player.Attack}");
            Console.WriteLine($"방어력 : {player.Defense}");
            Console.WriteLine($"체력   : {player.Health}/{player.MaxHealth}");
            Console.WriteLine($"Gold   : {player.Gold}");
            Console.WriteLine($"무기   : {player.EquippedWeapon?.Name ?? "없음"}");
            Console.WriteLine($"방어구 : {player.EquippedArmor?.Name ?? "없음"}");
            Console.WriteLine($"보조장비: {player.EquippedAccessory?.Name ?? "없음"}"); // 보조장비 표시

            Console.WriteLine("\n선택지로 돌아가려면 아무 키나 누르세요...");
            Console.ReadKey();
        }

        /*-----------------------------------------------아이템 클래스 및 인벤토리 관리-------------------------------------------------------------*/
        enum ItemType { Weapon, Armor, Accessory, Consumable } // 보조장비 유형 추가
        enum WeaponType { OneHanded, TwoHanded } // 무기 유형 추가 (한손검, 양손검)

        class Item
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsEquipped { get; set; } = false;
            public int Price { get; set; }
            public ItemType Type { get; set; }
            public WeaponType WeaponType { get; set; } // 무기 유형
            public float BonusAttack { get; set; } = 0;
            public float BonusDefense { get; set; } = 0;
            public int BonusHealth { get; set; } = 0;
            public int HealAmount { get; set; } = 0;

            public override string ToString()
            {
                string equippedIndicator = IsEquipped ? "[E] " : "";
                string bonusStats = "";

                if (BonusAttack > 0)
                    bonusStats += $" [공격력 +{BonusAttack}]";
                if (BonusDefense > 0)
                    bonusStats += $" [방어력 +{BonusDefense}]";
                if (BonusHealth > 0)
                    bonusStats += $" [체력 +{BonusHealth}]";

                return $"{equippedIndicator}{Name}{bonusStats} - {Description}";
            }

            public string ToSellString()
            {
                return $"{Name} - 판매 가격: {(int)(Price * 0.7)} Gold - {Description}";
            }

            public void UseItem(Player player)
            {
                if (Type == ItemType.Consumable && HealAmount > 0)
                {
                    int heal = Math.Min(HealAmount, player.MaxHealth - player.Health);
                    player.Health += heal;
                    Console.WriteLine($"{Name}을(를) 사용하여 체력을 {heal} 회복했습니다.");
                }
            }
        }

        static List<Item> inventory = new List<Item>
        {
            new Item { Name = "회복약", Description = "사용 시 체력을 회복해준다.", Price = 10, Type = ItemType.Consumable, HealAmount = 50 },
        };

        static void ManageInventory()
        {
            bool inManage = true;

            while (inManage)
            {
                Console.Clear();

                if (inventory.Count == 0)
                {
                    Console.WriteLine("인벤토리가 비어 있습니다.");
                    Console.WriteLine("0. 나가기");
                    Console.Write("원하는 행동을 선택하세요: ");

                    string input = Console.ReadLine();

                    if (input == "0")
                    {
                        inManage = false;
                    }
                    continue;
                }

                Console.WriteLine("\n=== 인벤토리 ===");
                for (int i = 0; i < inventory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {inventory[i]}");
                }
                Console.WriteLine("0. 나가기");

                Console.Write("아이템 번호를 선택하세요: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0)
                    {
                        inManage = false;
                    }
                    else if (choice >= 1 && choice <= inventory.Count)
                    {
                        var selectedItem = inventory[choice - 1];

                        if (selectedItem.Type == ItemType.Consumable)
                        {
                            if (player.Health < player.MaxHealth)
                            {
                                selectedItem.UseItem(player);
                                inventory.RemoveAt(choice - 1); // 소모품 사용 후 삭제
                            }
                            else
                            {
                                Console.WriteLine("체력이 가득 차 있어서 회복약을 사용할 수 없습니다.");
                            }
                        }
                        else if (selectedItem.Type == ItemType.Weapon || selectedItem.Type == ItemType.Armor || selectedItem.Type == ItemType.Accessory)
                        {
                            // 장착 아이템 처리
                            if (selectedItem.IsEquipped)
                            {
                                player.UnequipItem(selectedItem);
                            }
                            else
                            {
                                player.EquipItem(selectedItem);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{selectedItem.Name}은(는) 장착할 수 없는 아이템입니다.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }

        /*-----------------------------------------------상점-------------------------------------------------------------*/
        class ShopItem : Item
        {
            public bool IsPurchased { get; set; } = false;

            public override string ToString()
            {
                return IsPurchased ? $"{Name} (구매완료)" : $"{Name} - {Price} Gold - {Description}";
            }
        }

        static List<ShopItem> shopItems = new List<ShopItem>
        {
            new ShopItem { Name = "한손검", Description = "한 손으로 사용하는 검", Price = 30, Type = ItemType.Weapon, WeaponType = WeaponType.OneHanded, BonusAttack = 5 },
            new ShopItem { Name = "양손검", Description = "양 손으로 사용하는 강력한 검", Price = 50, Type = ItemType.Weapon, WeaponType = WeaponType.TwoHanded, BonusAttack = 10 },
            new ShopItem { Name = "방패", Description = "적의 공격을 막을 수 있는 방패", Price = 20, Type = ItemType.Accessory, BonusDefense = 3 },
            new ShopItem { Name = "갑옷", Description = "방어력을 높여주는 갑옷", Price = 50, Type = ItemType.Armor, BonusDefense = 10, BonusHealth = 20 },
            new ShopItem { Name = "회복약", Description = "사용 시 체력을 회복", Price = 10, Type = ItemType.Consumable, HealAmount = 50 }
        };

        static void VisitShop()
        {
            bool inShop = true;
            string statusMessage = "";

            while (inShop)
            {
                Console.Clear();

                Console.WriteLine($"현재 보유 골드: {player.Gold} Gold");
                Console.WriteLine("\n=== 상점 ===");

                for (int i = 0; i < shopItems.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {shopItems[i]}");
                }
                Console.WriteLine("9. 아이템 판매");
                Console.WriteLine("0. 나가기");

                if (!string.IsNullOrEmpty(statusMessage))
                {
                    Console.WriteLine($"\n{statusMessage}");
                    statusMessage = "";
                }

                Console.Write("구매할 아이템 번호를 선택하세요: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0)
                    {
                        inShop = false;
                    }
                    else if (choice == 9)
                    {
                        SellItem();
                    }
                    else if (choice >= 1 && choice <= shopItems.Count)
                    {
                        var selectedItem = shopItems[choice - 1];
                        if (selectedItem.IsPurchased)
                        {
                            statusMessage = "이미 구매한 아이템입니다.";
                        }
                        else if (player.Gold >= selectedItem.Price)
                        {
                            player.Gold -= selectedItem.Price;
                            selectedItem.IsPurchased = true;
                            inventory.Add(new Item
                            {
                                Name = selectedItem.Name,
                                Description = selectedItem.Description,
                                Price = selectedItem.Price,
                                Type = selectedItem.Type,
                                WeaponType = selectedItem.WeaponType,
                                BonusAttack = selectedItem.BonusAttack,
                                BonusDefense = selectedItem.BonusDefense,
                                BonusHealth = selectedItem.BonusHealth
                            });
                            statusMessage = $"{selectedItem.Name}을(를) 구매했습니다.";
                        }
                        else
                        {
                            statusMessage = "Gold가 부족합니다.";
                        }
                    }
                    else
                    {
                        statusMessage = "잘못된 입력입니다.";
                    }
                }
                else
                {
                    statusMessage = "잘못된 입력입니다.";
                }
            }
        }

        /*-----------------------------------------------아이템 판매-------------------------------------------------------------*/
        static void SellItem()
        {
            bool selling = true;
            while (selling)
            {
                Console.Clear();

                if (inventory.Count == 0)
                {
                    Console.WriteLine("판매할 아이템이 없습니다.");
                    return;
                }

                Console.WriteLine("\n=== 판매 가능한 아이템 ===");
                for (int i = 0; i < inventory.Count; i++)
                {
                    string equippedIndicator = inventory[i].IsEquipped ? "[E] " : "";
                    Console.WriteLine($"{i + 1}. {equippedIndicator}{inventory[i].ToSellString()}");
                }
                Console.WriteLine("0. 판매 종료");

                Console.Write("판매할 아이템 번호를 선택하세요: ");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= inventory.Count)
                {
                    var selectedItem = inventory[choice - 1];

                    if (selectedItem.IsEquipped)
                    {
                        Console.WriteLine("장착된 아이템은 판매할 수 없습니다.");
                        continue;
                    }

                    int sellPrice = (int)(selectedItem.Price * 0.85);

                    player.Gold += sellPrice;
                    inventory.RemoveAt(choice - 1);

                    Console.WriteLine($"{selectedItem.Name}을(를) {sellPrice} Gold에 판매했습니다.");
                }
                else if (choice == 0)
                {
                    selling = false;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }
}
