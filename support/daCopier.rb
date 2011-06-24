require 'FileUtils'

class DaCopier

  def initialize(exclude)
    @exclude = exclude
  end

  def copy src, dest

    stage dest

    Dir.foreach(src) do |file|
      next if exclude?(file)
	  next if file == "."
	  next if file == ".."

      s = File.join(src, file)
      d = File.join(dest, file)

      if File.directory?(s)
        FileUtils.mkdir(d)
        copy s, d
      else
        FileUtils.cp(s, d)
      end

      puts d

    end
  end

  private

  def stage dest
    if File.directory?(dest)
      FileUtils.rm_rf(dest)
    end

    FileUtils.mkdir(dest)
  end

  def exclude? file
    @exclude.each do |s|
      if file.match(/#{s}/i)
        return true
      end
    end
    false
  end
end