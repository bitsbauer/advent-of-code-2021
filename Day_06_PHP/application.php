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
        code: function (InputInterface $input, OutputInterface $output) {
            $getFishCountListFromInput = static function (string $filePath): array {
                $fishCountList = [0,0,0,0,0,0,0,0,0];
                $fishStatusList = array_map(static fn($item) => (int)$item, explode(',', file_get_contents($filePath)));
                foreach ($fishStatusList as $fishStatus) {
                    $fishCountList[$fishStatus] ++;
                }
                return $fishCountList;
            };

            $simulateFishPopulation = static function (array $fishCountList, int $days): int
            {
                for($day = 0; $day < $days; $day++) {
                    $nextFishCountList = [0,0,0,0,0,0,0,0,0];
                    for ($i = 7; $i >= 0; $i --) {
                        $nextFishCountList[$i] = $fishCountList[$i+1];
                    }
                    $nextFishCountList[8] = $fishCountList[0];
                    $nextFishCountList[6] += $fishCountList[0];
                    $fishCountList = $nextFishCountList;
                }
                return array_sum($fishCountList);
            };

            ###

            $output->writeln('Advent of Code 2021 - Day 6: Lanternfish (https://adventofcode.com/2021/day/6)');

            if (PHP_INT_SIZE !== 8) {
                throw new RuntimeException('Sorry, 64 Bit OS required...');
            }

            $filePath = $input->getArgument('input') ?? 'input.txt';
            $fishCountList = $getFishCountListFromInput($filePath);

            # Part 1
            $output->writeln('How many lanternfish would there be after 80 days?');
            $result = $simulateFishPopulation($fishCountList, 80);
            $output->writeln(sprintf('Result: %s', $result));

            # Part 2
            $output->writeln('How many lanternfish would there be after 256 days?');
            $result = $simulateFishPopulation($fishCountList, 256);
            $output->writeln(sprintf('Result: %s', $result));
        }
    )
    ->run();