// 11. О депутате. Депутата спрашивают одновременно 15 человек, каждый по 3
// вопроса. Депутат принимает один вопрос, думает 0,2 секунды и отвечает
// вопросившему. Всего депутат отвечает на 10 случайных вопросов от кого угодно
// (используйте MPI_ANY_SOURCE вместо ранга отправителя), затем прием
// оканчивается.

#include <iostream>
#include <cstdlib>
#include <mpi.h>
#include <unistd.h> 
#include <ctime>

#define NUM_VISITORS 15
#define QUESTIONS_PER_DEPUTY 3
#define TOTAL_QUESTIONS (NUM_VISITORS * QUESTIONS_PER_DEPUTY)
#define ANSWER_TIME 200000  // микросекунды

int main(int argc, char** argv) {
    int rank, size;
    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);

    srand(time(NULL));

    if (rank == 0) {
        std::cout << "Депутат отвечает на вопросы..." << std::endl;

        for (int i = 0; i < 10; ++i) {
            MPI_Status status;
            char question[10];

            // Депутат принимает любой вопрос от кого угодно
            MPI_Recv(question, sizeof(question), MPI_CHAR, MPI_ANY_SOURCE, 0, MPI_COMM_WORLD, &status);

            // Депутат думает перед ответом
            usleep(ANSWER_TIME);

            // Депутат отвечает
            std::cout << "Депутат ответил на вопрос:" << question << " от процесса " << status.MPI_SOURCE << std::endl;
        }
    }
    else {
        // Генерация и отправка вопросов
        for (int i = 0; i < QUESTIONS_PER_DEPUTY; ++i) {
            char question[10]{ (char)(i + 97), '?' };
            std::cout << "Посетитель " << rank << " задал вопрос " << i + 1 << std::endl;
            MPI_Send(question, sizeof(question), MPI_CHAR, 0, 0, MPI_COMM_WORLD);
        }
    }

    MPI_Finalize();
    return EXIT_SUCCESS;
}
