include FileTest

require 'rubygems'
require 'zip/zip'
require 'zip/zipfilesystem'
require 'albacore'
require 'rexml/document'
include REXML
require 'FileUtils'

require "support/buildUtils.rb"

ROOT_NAMESPACE = "Scratchpad"
RESULTS_DIR = "build/test-reports"
BUILD_NUMBER_BASE = "0.1.0"
PRODUCT = ROOT_NAMESPACE
COPYRIGHT = 'Copyright Joshua Arnold 2011. All rights reserved.';
COMMON_ASSEMBLY_INFO = 'src/CommonAssemblyInfo.cs';
CLR_VERSION = "v4.0"
COMPILE_TARGET = "Debug"

props = { :archive => "build", :stage => "stage" }

desc "Compiles and runs unit tests"
task :all => [:default]

desc "**Default**, compiles and runs tests"
task :default => [:compile, :unit_tests]

desc "Update the version information for the build"
assemblyinfo :version do |asm|
  asm_version = BUILD_NUMBER_BASE + ".0"
  
  begin
	gittag = `git describe --long`.chomp 	# looks something like v0.1.0-63-g92228f4
    gitnumberpart = /-(\d+)-/.match(gittag)
    gitnumber = gitnumberpart.nil? ? '0' : gitnumberpart[1]
    commit = `git log -1 --pretty=format:%H`
  rescue
    commit = "git unavailable"
    gitnumber = "0"
  end
  build_number = "#{BUILD_NUMBER_BASE}.#{gitnumber}"
  tc_build_number = ENV["BUILD_NUMBER"]
  puts "##teamcity[buildNumber '#{build_number}-#{tc_build_number}']" unless tc_build_number.nil?
  asm.trademark = commit
  asm.product_name = "#{PRODUCT} #{gittag}"
  asm.description = build_number
  asm.version = asm_version
  asm.file_version = build_number
  asm.custom_attributes :AssemblyInformationalVersion => asm_version
  asm.copyright = COPYRIGHT
  asm.output_file = COMMON_ASSEMBLY_INFO
end

desc "Prepares the working directory for a new build"
task :clean do	
	puts("recreating the build directory")
	buildDir = props[:archive]
	stageDir = props[:stage]
	FileUtils.rm_r(Dir.glob(File.join(buildDir, '*')), :force=>true) if exists?(buildDir)
	FileUtils.rm_r(Dir.glob(File.join(stageDir, '*')), :force=>true) if exists?(stageDir)
	
	FileUtils.rm_r(Dir.glob(buildDir), :force=>true) if exists?(buildDir)
	FileUtils.rm_r(Dir.glob(stageDir), :force=>true) if exists?(stageDir)
	
	Dir.mkdir buildDir unless exists?(buildDir)
	Dir.mkdir stageDir unless exists?(stageDir)
	Dir.mkdir RESULTS_DIR unless exists?(RESULTS_DIR)
end

desc "Compiles the app"
msbuild :compile => [:clean, :version] do |msb|
  msb.properties :configuration => COMPILE_TARGET
  msb.targets :Clean, :Build
  msb.solution = "src/#{ROOT_NAMESPACE}.sln"
end

desc "Runs unit tests"
task :unit_tests do
  runner = NUnitRunner.new :compilemode => COMPILE_TARGET, :source => 'src', :platform => 'x86', :results => RESULTS_DIR
  runner.executeTests []
end