using System;

class PenaltyKickGame
{
    static void Main(string[] args)
    {
        Random random = new Random();
        int totalSuccess = 0;
        int numAttempts = 5;

        // 강화 시도 횟수와 성공 횟수를 저장할 변수 추가
        int enhancementAttempts = 0;
        int enhancementSuccess = 0;

        for (int i = 0; i < numAttempts; i++)
        {
            // 키커의 방향 선택
            Console.WriteLine("킥 방향을 선택하세요 (왼쪽: A, 중앙: S, 오른쪽: D):");
            char kickerDirection = Char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            // 입력된 키가 A, S, D 중 하나인지 확인
            if (kickerDirection != 'A' && kickerDirection != 'S' && kickerDirection != 'D')
            {
                Console.WriteLine("잘못된 입력입니다. A, S, D 중 하나를 입력하세요.");
                continue; // 잘못된 입력이면 다음 반복으로 넘어감
            }
            
            // 강화 시도 후 골키퍼의 랜덤한 방향 설정
            int goalkeeperDirection;
            if (enhancementAttempts > 0)
            {
                // 성공 했을 경우 골키퍼가 왼쪽으로 뛰는 확률을 감소시킴
                goalkeeperDirection = random.Next(0, 90);
            }
            else
            {
                goalkeeperDirection = random.Next(0, 100);
            }

            string goalkeeperDirectionText;
            
            if (goalkeeperDirection < 33)
                goalkeeperDirectionText = "왼쪽";
            else if (goalkeeperDirection < 66)
                goalkeeperDirectionText = "중앙";
            else
                goalkeeperDirectionText = "오른쪽";
            
            Console.WriteLine($"골키퍼가 {goalkeeperDirectionText}으로 뛰었습니다.");
            
            // 성공 여부 확인
            bool isSuccess = (kickerDirection != 'A' && goalkeeperDirection < 33) ||
                             (kickerDirection != 'S' && goalkeeperDirection >= 33 && goalkeeperDirection < 66) ||
                             (kickerDirection != 'D' && goalkeeperDirection >= 66);

            // 결과 출력
            if (isSuccess)
            {
                Console.WriteLine("성공!");
                totalSuccess++;

                // 성공 시 강화 시도 후 성공 확률 증가
                enhancementAttempts++;
                enhancementSuccess++;
            }
            else
            {
                Console.WriteLine("실패!");

                // 실패 시 강화 시도 후 성공 확률 초기화
                enhancementAttempts = 0;
                enhancementSuccess = 0;
            }
        }

        // 최종 결과 출력
        Console.WriteLine($"게임 종료! 총 {numAttempts}번 중 {totalSuccess}번 성공하였습니다.");
    }
}
