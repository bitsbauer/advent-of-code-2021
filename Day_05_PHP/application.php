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
            $getVentLines = static function (string $filePath): array {
                $lines = explode("\n", file_get_contents($filePath));
                $ventLines = [];
                foreach ($lines as $line) {
                    $coords = explode(" -> ", $line);
                    $start = array_map(static fn($item) => (int)$item, explode(',', $coords[0]));
                    $end = array_map(static fn($item) => (int)$item, explode(',', $coords[1]));
                    $ventLines[] = [$start, $end];
                }
                return $ventLines;
            };

            $isHorizontalOrVertical = static function (array $ventLine): bool {
                return $ventLine[0][0] === $ventLine[1][0] || $ventLine[0][1] === $ventLine[1][1];
            };

            $applyVentLineToGrid = static function (array &$grid, array $ventLine): void {
                if ($ventLine[0][0] === $ventLine[1][0]) {
                    $startY = $ventLine[0][1];
                    $endY = $ventLine[1][1];
                    if ($startY > $endY) {
                        $startY = $ventLine[1][1];
                        $endY = $ventLine[0][1];
                    }
                    for($y= $startY; $y <= $endY; $y ++) {
                        @$grid[$ventLine[0][0]][$y] ++;
                    }
                }
                if ($ventLine[0][1] === $ventLine[1][1]) {
                    $startX = $ventLine[0][0];
                    $endX = $ventLine[1][0];
                    if ($startX > $endX) {
                        $startX = $ventLine[1][0];
                        $endX = $ventLine[0][0];
                    }
                    for($x= $startX; $x <= $endX; $x ++) {
                        @$grid[$x][$ventLine[1][1]] ++;
                    }
                }
            };

            $applyDiaVentLineToGrid = static function (array &$grid, array $ventLine): void {
                $startX = $ventLine[0][0];
                $endX = $ventLine[1][0];

                $startY = $ventLine[0][1];
                $endY = $ventLine[1][1];

                $y= $startY;
                for($x= $startX; ($startX > $endX) ? $x >= $endX : $x <= $endX; ($startX > $endX) ? $x -- : $x ++) {
                    @$grid[$x][$y] ++;
                    ($startY > $endY) ? $y -- : $y ++;
                }
            };

            ###

            $output->writeln('Advent of Code 2021 - Day 5: Hydrothermal Venture (https://adventofcode.com/2021/day/5)');

            if (PHP_INT_SIZE !== 8) {
                throw new RuntimeException('Sorry, 64 Bit OS required...');
            }

            $filePath = $input->getArgument('input') ?? 'input.txt';

            # Part 1
            $output->writeln('At how many points do at least two lines overlap?');
            $grid = [];
            $ventLines = $getVentLines($filePath);
            foreach ($ventLines as $ventLine) {
                if($isHorizontalOrVertical($ventLine)) {
                    $applyVentLineToGrid($grid, $ventLine);
                }
            }
            $count = 0;
            foreach ($grid as $gridLine) {
                foreach ($gridLine as $value) {
                    if ($value >= 2) {
                        $count++;
                    }
                }
            }
            $output->writeln(sprintf('Result: %s', $count));

            # Part 2
            $output->writeln('At how many points do at least two lines overlap?');
            $grid = [];
            $ventLines = $getVentLines($filePath);
            foreach ($ventLines as $ventLine) {
                if($isHorizontalOrVertical($ventLine)) {
                    $applyVentLineToGrid($grid, $ventLine);
                } else {
                    $applyDiaVentLineToGrid($grid, $ventLine);
                }
            }
            $count = 0;
            foreach ($grid as $gridLine) {
                foreach ($gridLine as $value) {
                    if ($value >= 2) {
                        $count++;
                    }
                }
            }
            $output->writeln(sprintf('Result: %s', $count));
        }
    )
    ->run();