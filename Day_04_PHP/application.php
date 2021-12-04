#!/usr/bin/env php
<?php

declare(strict_types=1);

require __DIR__ . '/vendor/autoload.php';

use Symfony\Component\Console\Input\InputArgument;
use Symfony\Component\Console\Input\InputInterface;
use Symfony\Component\Console\Output\OutputInterface;
use Symfony\Component\Console\SingleCommandApplication;

(new SingleCommandApplication())
    ->setName("Advent of Code 2021")
    ->addArgument('input', InputArgument::OPTIONAL, 'Specify input file, default: input.txt')
    ->setCode(
        function (InputInterface $input, OutputInterface $output) {
            $getBingoNumbers = static function (string $filePath): array {
                $inputParts = explode("\n\n", file_get_contents($filePath));
                $bingoNumbersInput = array_shift($inputParts);
                return array_map(static fn($item) => (int)$item, explode(',', $bingoNumbersInput));
            };

            $getBingoBoards = static function (string $filePath): array {
                $bingoBoardsData = explode("\n\n", file_get_contents($filePath));
                array_shift($bingoBoardsData);
                $bingoBoards = [];
                foreach ($bingoBoardsData as $bingoBoardData) {
                    $bingoBoard = [];
                    $lines = explode("\n", $bingoBoardData);
                    foreach ($lines as $lineId => $line) {
                        $bingoBoard[] = array_map(static fn($item) => (int)$item, explode(' ', str_replace('  ', ' ', trim($line))));
                    }
                    $bingoBoards[] = $bingoBoard;
                }
                return $bingoBoards;
            };

            $isBingo = static function (array $bingoBoard): bool {
                $cols = [];
                foreach ($bingoBoard as $rowIndex => $row) {
                    $rowIsBingo = true;
                    foreach ($row as $columnIndex => $value) {
                        $cols[$columnIndex][$rowIndex] = $value;
                        if ($value > 0) {
                            $rowIsBingo = false;
                        }
                    }
                    if ($rowIsBingo) {
                        return true;
                    }
                }
                foreach ($cols as $col) {
                    $colIsBingo = true;
                    foreach ($col as $value) {
                        if ($value > 0) {
                            $colIsBingo = false;
                        }
                    }
                    if ($colIsBingo) {
                        return true;
                    }
                }
                return false;
            };

            $markBingoBoard = static function(int $bingoNumber, array $bingoBoard): array {
                foreach ($bingoBoard as $rowIndex => $row) {
                    foreach ($row as $columnIndex => $value) {
                        if($value === $bingoNumber) {
                            $bingoBoard[$rowIndex][$columnIndex] = -1;
                        }
                    }
                }
                return $bingoBoard;
            };

            $findResultOne = static function($filePath) use ($getBingoNumbers, $getBingoBoards, $isBingo, $markBingoBoard): ?int {
                $bingoNumbers = $getBingoNumbers($filePath);
                $bingoBoards = $getBingoBoards($filePath);
                foreach ($bingoNumbers as $bingoNumber) {
                    foreach ($bingoBoards as $bingoBoardsIndex => $bingoBoard) {
                        $markedBingoBoard = $markBingoBoard($bingoNumber, $bingoBoard);
                        if ($isBingo($markedBingoBoard)) {
                            $sum = 0;
                            foreach ($markedBingoBoard as $row) {
                                foreach ($row as $value) {
                                    if($value > -1) {
                                        $sum += $value;
                                    }
                                }
                            }
                            return $sum * $bingoNumber;
                        }
                        $bingoBoards[$bingoBoardsIndex] = $markedBingoBoard;
                    }
                }
                return null;
            };

            $findResultTwo = static function($filePath) use ($getBingoNumbers, $getBingoBoards, $isBingo, $markBingoBoard): ?int {
                $bingoNumbers = $getBingoNumbers($filePath);
                $bingoBoards = $getBingoBoards($filePath);
                $bingoBoardsWon = [];
                foreach ($bingoNumbers as $bingoNumber) {
                    foreach ($bingoBoards as $bingoBoardsIndex => $bingoBoard) {
                        if (in_array($bingoBoardsIndex, $bingoBoardsWon, true)) {
                            continue;
                        }
                        $markedBingoBoard = $markBingoBoard($bingoNumber, $bingoBoard);
                        if ($isBingo($markedBingoBoard)) {
                            $bingoBoardsWon[] = $bingoBoardsIndex;
                            if (count($bingoBoardsWon) < count($bingoBoards)) {
                                continue;
                            }
                            $sum = 0;
                            foreach ($markedBingoBoard as $row) {
                                foreach ($row as $value) {
                                    if($value > -1) {
                                        $sum += $value;
                                    }
                                }
                            }
                            return $sum * $bingoNumber;
                        }
                        $bingoBoards[$bingoBoardsIndex] = $markedBingoBoard;
                    }
                }
                return null;
            };

            ###

            $output->writeln('Advent of Code 2021 - Day 4: Giant Squid (https://adventofcode.com/2021/day/4)');

            if (PHP_INT_SIZE !== 8) {
                throw new RuntimeException('Sorry, 64 Bit OS required...');
            }

            $filePath = $input->getArgument('input') ?? 'input.txt';

            # Part 1
            $output->writeln('What will your final score be if you choose that board?');
            $result = $findResultOne($filePath);
            $output->writeln(sprintf('Result: %s', $result));

            # Part 2
            $output->writeln('Figure out which board will win last. Once it wins, what would its final score be?');
            $result = $findResultTwo($filePath);
            $output->writeln(sprintf('Result: %s', $result));
        }
    )
    ->run();