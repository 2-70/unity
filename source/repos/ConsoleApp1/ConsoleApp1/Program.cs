using System;
using System.Collections.Generic;
using static Shop;

public class Item
{
    public string Name { get; }
    public string Status0 { get; }
    public int Status { get; }
    public string Description { get; }
    public int Price { get; }

    public Item(string name, string status0, int status, string description, int price)
    {
        Name = name;
        Status0 = status0;
        Status = status;
        Description = description;
        Price = price;
    }
}

public class Inventory
{
    private Player player;
    public Inventory(Player player)
    {
        this.player = player;
    }

    private List<Item> items = new List<Item>
    {
        new Item("무쇠갑옷", "방어력", +5, "무쇠로 만들어져 튼튼한 갑옷입니다.", 0),
        new Item("스파르타의 창", "공격력", +7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 0),
        new Item("낡은 검", "공격력", +2, "쉽게 볼 수 있는 낡은 검입니다.", 0),
    };

    // 여러 개의 장착 아이템 지원
    private List<Item> equippedItems = new List<Item>();

    public void ShowInventory()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]\n");

            foreach (var item in items)
            {
                if (equippedItems.Contains(item))
                    Console.WriteLine($"-[E] {item.Name} | {item.Status0} +{item.Status} | {item.Description}");
                else
                    Console.WriteLine($"- {item.Name} | {item.Status0} +{item.Status} | {item.Description}");
            }

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요. \n>> ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        ManageEquip();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 아무 키나 누르세요.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    private void AdjustPlayerStat(Item item, int multiplier)
    {
        if (item.Status0 == "공격력")
        {
            player.Attack += item.Status * multiplier;
        }
        else if (item.Status0 == "방어력")
        {
            player.Defense += item.Status * multiplier;
        }
    }

    private void ManageEquip()
    {
        Console.Clear();
        Console.WriteLine("인벤토리 - 장착관리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < items.Count; i++)
        {
            string equippedText = equippedItems.Contains(items[i]) ? "[E]" : "";
            Console.WriteLine($"- {i + 1}.{equippedText}{items[i].Name} | {items[i].Status0} +{items[i].Status} | {items[i].Description}");
        }

        Console.WriteLine("\n0. 나가기\n");
        Console.Write("원하시는 행동을 입력해주세요. \n>> ");
        if (int.TryParse(Console.ReadLine(), out int input))
        {
            if (input == 0) return;

            if (input >= 1 && input <= items.Count)
            {
                Item selectedItem = items[input - 1];

                if (equippedItems.Contains(selectedItem))
                {
                    // 이미 장착된 경우 해제
                    equippedItems.Remove(selectedItem);
                    AdjustPlayerStat(selectedItem, -1);
                    Console.WriteLine($"\n'{selectedItem.Name}' 장착을 해제했습니다!");
                }
                else
                {
                    // 새로 장착
                    equippedItems.Add(selectedItem);
                    AdjustPlayerStat(selectedItem, +1);
                    Console.WriteLine($"\n'{selectedItem.Name}'을(를) 장착했습니다!");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
        else
        {
            Console.WriteLine("숫자를 입력해주세요.");
        }

        Console.WriteLine("계속하려면 아무 키나 누르세요.");
        Console.ReadKey();
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }
}


public class Player
{
    public string Name;
    public string Job;
    public int Level;
    public int Defense;
    public int Hp;
    public int Attack;
    public int Gold;


    public Player(string name)
    {
        Name = name;
        Level = 1;
        Attack = 10;
        Defense = 5;
        Hp = 100;
        Gold = 1500;
        Job = "전사";
    }

    public void ShowStatus()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.WriteLine($"Lv. {Level}");
            Console.WriteLine($"{Name} ({Job})");
            Console.WriteLine($"공격력 : {Attack}");
            Console.WriteLine($"방어력 : {Defense}");
            Console.WriteLine($"체  력 : {Hp}");
            Console.WriteLine($"Gold   : {Gold} G\n");

            Console.WriteLine("0.나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
            string input = Console.ReadLine();

            if (input == "0") break;
            else
            {
                Console.WriteLine("\n잘못된 입력입니다.");
                Console.ReadKey();
            }
        }
    }
}

public class Shop
{
    private List<Item> purchasedItems = new List<Item>();
    

    private List<Item> shopItems = new List<Item>
    {
        new Item("수련자 갑옷", "방어력", +5, "수련에 도움을 주는 갑옷입니다.", 1000),
        new Item("무쇠갑옷", "방어력", +9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 0),
        new Item("스파르타의 갑옷", "방어력", +15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
        new Item("낡은 검", "공격력", +2, "쉽게 볼 수 있는 낡은 검입니다.", 600),
        new Item("청동 도끼", "공격력", +5, "어디선가 사용됐던거 같은 도끼입니다.", 1500),
        new Item("스파르타의 창", "공격력", +7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 0)
    };

    public Shop()
    {
        // 초기 구매 처리할 아이템을 인덱스로 지정
        purchasedItems.Add(shopItems[1]); // 무쇠갑옷
        purchasedItems.Add(shopItems[5]); // 스파르타의 창
    }

    public void ShowShop(Player player, Inventory inventory)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine($"[보유 골드] \n{player.Gold} G\n");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < shopItems.Count; i++)
            {
                var item = shopItems[i];
                string priceText = purchasedItems.Contains(item) ? "구매완료" : $"{item.Price} G";

                Console.WriteLine($"{i + 1}. {item.Name} | {item.Status0} +{item.Status} | {item.Description} | {priceText}");
            }

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요. \n>> ");

            string input = Console.ReadLine();

            if (input == "0")
            {
                break;
            }
            else if (input == "1")
            {
                BuyItem(player, inventory);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
            }
        }
    }

    private void BuyItem(Player player, Inventory inventory)
    {
        Console.Clear();
        Console.WriteLine("상점 - 아이템 구매");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
        Console.WriteLine($"[보유 골드] \n{player.Gold} G\n");
        Console.WriteLine($"[아이템 목록]");

        for (int i = 0; i < shopItems.Count; i++)
        {
            var item = shopItems[i];
            string priceText = purchasedItems.Contains(item) ? "구매완료" : $"{item.Price} G";
            Console.WriteLine($"{i + 1}. {item.Name} | {item.Status0} +{item.Status} | {item.Description} | {priceText}");
        }

        Console.WriteLine("\n0. 돌아가기");
        Console.Write("구매할 아이템 번호를 입력하세요 \n>> ");

        if (int.TryParse(Console.ReadLine(), out int input))
        {
            if (input == 0) return;

            if (input >= 1 && input <= shopItems.Count)
            {
                Item selectedItem = shopItems[input - 1];

                if (purchasedItems.Contains(selectedItem))
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else if (player.Gold >= selectedItem.Price)
                {
                    player.Gold -= selectedItem.Price;
                    inventory.AddItem(selectedItem);
                    purchasedItems.Add(selectedItem); // 구매 목록 추가
                    Console.WriteLine($"'{selectedItem.Name}'을(를) 구매했습니다!");
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다..");
            }
        }
        else
        {
            Console.WriteLine("숫자를 입력해주세요.");
        }

        Console.WriteLine("계속하려면 아무 키나 누르세요.");
        Console.ReadKey();
    }


    public class Stage
    {
        private Player player;
        private Inventory inventory;
        private Shop shop;

        public Stage(Player player, Inventory inventory)
        {
            this.player = player;
            this.inventory = inventory;
            this.shop = new Shop(); // 상점 생성
        }

        public void GetSelect()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("C# 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("0. 종료\n");
                Console.Write("원하시는 행동을 입력해주세요.\n>> ");

                if (int.TryParse(Console.ReadLine(), out int selectInput))
                {
                    switch (selectInput)
                    {
                        case 1:
                            player.ShowStatus();
                            break;
                        case 2:
                            inventory.ShowInventory();
                            break;
                        case 3:
                            shop.ShowShop(player, inventory);
                            break;
                        case 0:
                            Console.WriteLine("게임을 종료합니다.");
                            return;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }
    }

    
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("플레이어 이름을 입력하세요: ");
        string playerName = Console.ReadLine();


        Player player = new Player(playerName);
        Inventory inventory = new Inventory(player);
        Stage stage = new Stage(player, inventory);

        stage.GetSelect();
    }
}
