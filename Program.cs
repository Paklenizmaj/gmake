using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace gmake
{
    class Program
    {
        private static string projectName;
        private static string projectDirectory;

        static void CreateDirectory(string dirPath)
        {
            Console.WriteLine("~ mkdir " + dirPath);
            Directory.CreateDirectory(dirPath);
        }

        static void WriteCMakeLists()
        {
            const string cmlplaceholder = @"project(#{PROJECT_NAME})
cmake_minimum_required(VERSION 2.6)

include_directories(${#{PROJECT_NAME}_SOURCE_DIR}/include)

file(GLOB_RECURSE cppFiles src/*.cpp)

add_executable(#{PROJECT_NAME} ${cppFiles})";

            StreamWriter sw = File.CreateText(projectDirectory + @"CMakeLists.txt");
            sw.Write(cmlplaceholder.Replace("#{PROJECT_NAME}", projectName));
            sw.Close();
        }

        static void CreateHelloWorld()
        {
            const string code = @"#include <iostream>

int main()
{
    std::cout << " + "\"Hello World\"" + @";
    return 0;
}";
            StreamWriter sw = File.CreateText(projectDirectory + @"src\main.cpp");
            sw.Write(code);
            sw.Close();
        }

        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Too many arguments");
                return;
            }

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: gmake <project_name>");
                return;
            }

            projectName = args[0];
            projectDirectory = Directory.GetCurrentDirectory() + @"\" + projectName + @"\";

            CreateDirectory(projectDirectory);
            CreateDirectory(projectDirectory + "build");
            CreateDirectory(projectDirectory + "include");
            CreateDirectory(projectDirectory + "src");

            WriteCMakeLists();
            CreateHelloWorld();

            Console.WriteLine("Running cd " + projectName + "\\build && cmake .. -G \"MinGW Makefiles\"");
            System.Threading.Thread.Sleep(2000);
            System.Diagnostics.Process.Start("cmd.exe", "/C cd " + projectName + "\\build && cmake .. -G \"MinGW Makefiles\"");

            Console.WriteLine("Running cd " + projectName + "\\build && cmake ..");
            System.Threading.Thread.Sleep(4000);
            System.Diagnostics.Process.Start("cmd.exe", "/C cd " + projectName + "\\build && cmake ..");
        }
    }
}
