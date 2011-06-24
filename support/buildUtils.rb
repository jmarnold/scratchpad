require 'erb'

class NUnitRunner
	include FileTest

	def initialize(paths)
		@sourceDir = paths.fetch(:source, 'source')
		@resultsDir = paths.fetch(:results, 'results')
		@compilePlatform = paths.fetch(:platform, '')
		@compileTarget = paths.fetch(:compilemode, 'debug')
	
		@nunitExe = File.join('lib', 'NUnit', "nunit-console#{(@compilePlatform.empty? ? '' : "-#{@compilePlatform}")}.exe").gsub('/','\\') + ' /nothread'
	end
	
	def executeTests(assemblies)
		Dir.mkdir @resultsDir unless exists?(@resultsDir)
		
		assemblies.each do |assem|
			file = File.expand_path("#{@sourceDir}/#{assem}/bin/#{@compileTarget}/#{assem}.dll")
			sh "#{@nunitExe} \"#{file}\" /xml=#{@resultsDir}/#{assem}-TestResults.xml"
		end
	end
end