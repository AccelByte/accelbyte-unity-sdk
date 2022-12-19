# Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
# This is licensed software from AccelByte Inc, for limitations
# and restrictions contact your company contract manager.

import re
import sys
import json
import shutil
import subprocess

from enum import Enum
from pathlib import Path
from typing import List


class DocsType(Enum):
    INTERNAL = 0
    PUBLIC = 1


DOXYGEN_EXE = "doxygen.exe"

DOCS_TYPE_DICT = {
    "internal": DocsType.INTERNAL,
    "public": DocsType.PUBLIC
}


def get_app_version(exe_name: str) -> str:
    """Get version of the executable by running it wit --version argument

    Args:
        exe_name (str): name or fullpath to the executable

    Returns:
        str: version of the executable, None if the executable not found
    """
    try:
        ret = subprocess.run([exe_name, "--version"], capture_output=True)
        version = ret.stdout.decode("utf-8").strip()
        return version

    except FileNotFoundError:
        return None


def run_app(exe_name: str, args: List[str] = [], work_dir: Path = None) -> bool:
    """Run any installed application on the machine

    Args:
        exe_name (str): name or fullpath to the executable
        args (List[str], optional): arguments for the executable. Defaults to [].
        work_dir (Path, optional): working directory when running the executable. Defaults to None.

    Returns:
        bool: True if the executable exit with 0 returncode
    """
    try:
        exe_args = [exe_name]
        exe_args.extend(args)

        ret = subprocess.run(exe_args, cwd=work_dir)
        return ret.returncode == 0

    except ModuleNotFoundError:
        return False


def get_files(path: Path, pattern: str, recursive: bool = False) -> List[Path]:
    """Get list of files path in a directory with certain pattern (can accept wild card)

    Args:
        path (Path): Path to search for file(s)
        pattern (str): string indicating pattern of file e.g *.cpp
        recursive (bool, optional): Turn on to search recursively. Defaults to False.

    Returns:
        List[Path]: List of file(s) path found
    """
    file_list = []

    glob_result = None
    if recursive:
        glob_result = path.rglob(pattern)
    else:
        glob_result = path.glob(pattern)

    for f in glob_result:
        file_list.append(f)

    return file_list


class AccelByteDocsBuilderConfig:
    """Object representation of docs_config.json
    """

    def __init__(self, docs_root: Path) -> None:
        """ Load docs_config.json from specified directory. The directory is working directory of docs-builder.

        Args:
            docs_root (Path): Path where docs_config.json located
        """
        self.docs_root = docs_root

        data = None
        with open(self.docs_root.joinpath("docs_config.json"), "r") as f:
            data = json.load(f)

        self.doxyfile_static = data["doxyfile_static"]

        self.version_placeholder = data["version_placeholder"]

        self.doxygen_work_dir = self.docs_root.joinpath(
            data["doxygen_work_dir"])

        self.doxygen_input = []
        for entry in self.doxyfile_static["@INPUT@"]:
            entry_path = self.doxygen_work_dir.joinpath(entry).resolve()
            self.doxygen_input.append(entry_path)

        self.doxyfile_in = self.doxygen_work_dir.joinpath(data["doxyfile_in"])
        self.doxyfile_out = self.doxygen_work_dir.joinpath(
            data["doxyfile_out"])
        self.doxygen_html_out_dir = self.doxygen_work_dir.joinpath(
            data["doxygen_html_out_dir"])


class AccelByteDocsPreProcessor:
    """Pre-processor for markdown file to process unmatched quote
    """
    backup_extension = ".bak"

    def __init__(self, pre_processor_input: List[Path]) -> None:
        """Instantiate Docs Pre-processor

        Args:
            files (List[Path]): List of Path to markdown files
        """
        self._files = []
        for file in pre_processor_input:
            if file.is_file() and file.name.endswith(".md"):
                self._files.append(file)

    def run(self):
        """Storing original file(s) backup in *.bak extension and start pre-processing
        """
        self._backup()
        self._process_unmatched_quotes()

    def restore(self):
        """Restore file(s) to original
        """
        for file in self._files:
            backup_filepath = file.parent.joinpath(
                f"{file.name}{self.backup_extension}")
            shutil.move(backup_filepath, file)

    def _process_unmatched_quotes(self):
        for file in self._files:
            filepath_in = file.parent.joinpath(
                f"{file.name}{self.backup_extension}")

            with open(filepath_in, "r") as f_in, open(file, "w") as f_out:
                for line in f_in:
                    line_out = ""

                    double_quote_counts = line.count('"')
                    if double_quote_counts % 2 != 0:
                        line_out = f"{line.rstrip()}\"\n"
                    else:
                        line_out = line

                    f_out.writelines(line_out)

    def _backup(self):
        for file in self._files:
            backup_filepath = file.parent.joinpath(
                f"{file.name}{self.backup_extension}")
            shutil.move(file, backup_filepath)


class AccelByteDocsPostProcessor:
    def __init__(self, work_dir: Path) -> None:
        self.work_dir = work_dir

        self._file_list = []

    def run(self):
        self._file_list = get_files(self.work_dir, "md__*", recursive=True)

        self._replace_string("&lt;", "<")
        self._replace_string("&gt;", ">")

        self._process_navtreedata()

        self._create_autolink()

    def _create_autolink(self):
        with open(self.work_dir.joinpath("index.html"), "w") as f:
            f.writelines(
                "<head><meta http-equiv='refresh' content='0; URL=html/index.html'></head>")

    def _replace_string(self, search: str, replace_with: str):
        for filepath in self._file_list:
            post_processed_data = None

            with open(filepath, "r") as f:
                data = f.read()
                post_processed_data = data.replace(search, replace_with)

            with open(filepath, "w") as f:
                f.write(post_processed_data)

    def _process_navtreedata(self):
        input_file = self.work_dir.joinpath("html/navtreedata.js")

        navtree_entry_pattern = r'<a.*compare.*a>'
        version_pattern = r'>[0-9]+\.[0-9]+\.[0-9]+<'

        out_data = []

        with open(input_file, "r") as f:
            for line in f:
                match = re.search(navtree_entry_pattern, line)

                if match:
                    search_str = match.group()
                    version_str = re.search(
                        version_pattern, search_str).group()[1:-1]
                    line_out = line.replace(search_str, version_str)
                else:
                    line_out = line

                out_data.append(line_out)

        with open(input_file, "w") as f:
            f.writelines(out_data)


class AccelByteDocsBuilder:
    """Docs builder based on doxygen
    """

    def __init__(self, config: AccelByteDocsBuilderConfig, version: str) -> None:
        """Initialize docs builder

        Args:
            config (AccelByteDocsBuilderConfig): Config for builder
            version (str): version string that will be used to generate HTML in doxygen
        """
        self._config = config
        self._version = version
        self._post_processor = AccelByteDocsPostProcessor(
            config.doxygen_html_out_dir)

    def build(self, docs_type: DocsType, output_dir: Path = None) -> bool:
        """Start building HTML documents using doxygen

        Args:
            docs_type (DocsType): internal/public
            output_dir (Path, optional): Output directory for HTML documents. Defaults to None.

        Returns:
            bool: True if success
        """
        if not self._check_prerequisites():
            print("prerequisites are not met, skipping documents build")
            return False

        is_success = False

        pre_processor = AccelByteDocsPreProcessor(
            self._config.doxygen_input)
        pre_processor.run()

        if docs_type == DocsType.PUBLIC:
            is_success = self._generate_public_docs()
        elif docs_type == DocsType.INTERNAL:
            is_success = self._generate_internal_docs()

        if is_success:
            self._post_processor.run()
            self._move_output(output_dir)

        pre_processor.restore()

        return is_success

    def _move_output(self, output_dir: Path):
        if output_dir is not None:
            if output_dir.exists():
                shutil.rmtree(output_dir)

            shutil.move(self._config.doxygen_html_out_dir, output_dir)

    def _generate_public_docs(self) -> bool:
        is_doxygen_generated = False
        if self._process_doxyfile_input():
            is_doxygen_generated = self._generate_doxygen_docs()
            return is_doxygen_generated
        else:
            return False

    def _generate_internal_docs(self) -> bool:
        return self._generate_public_docs()

    def _process_doxyfile_input(self) -> bool:
        try:
            doxyfile_content = ""

            with open(self._config.doxyfile_in, "r") as f_in:
                doxyfile_content = f_in.read()

            doxyfile_content = doxyfile_content.replace(self._config.version_placeholder, self._version)

            for key, val in self._config.doxyfile_static.items():
                if isinstance(val, List):
                    out_str = " ".join(val)
                else:
                    out_str = val

                doxyfile_content = doxyfile_content.replace(key, out_str)

            with open(self._config.doxyfile_out, "w") as f_out:
                f_out.write(doxyfile_content)

            print(f"{self._config.doxyfile_out} generated")
            return True

        except FileNotFoundError as e:
            print(e)
            return False

    def _generate_doxygen_docs(self) -> bool:
        print("start building doxygen documents...")
        result = run_app(DOXYGEN_EXE,
                         work_dir=self._config.doxygen_work_dir)
        print("doxygen documents built successfully")
        return result

    def _check_prerequisites(self) -> bool:
        doxygen_version = get_app_version(f"{DOXYGEN_EXE}")

        print(f"{DOXYGEN_EXE}: {doxygen_version}")

        if doxygen_version is None:
            return False

        return True


def show_help():
    print(f"Usage: python {sys.argv[0]} <public/internal> <version>")
    print(f"Example: python {sys.argv[0]} public 14.1.7")


def main() -> int:
    if len(sys.argv) < 2:
        show_help()
        return 0

    docs_type = None
    docs_version = "development-build"
    try:
        docs_type = DOCS_TYPE_DICT[sys.argv[1]]
        docs_version = sys.argv[2]

    except KeyError:
        print("Document type is not supported please select public/internal")
        return -1

    except IndexError:
        print(
            f"Document version is not specified, using {docs_version} as version")

    script_root_path = Path(sys.argv[0]).parent.resolve()

    docs_root_path = script_root_path.joinpath("..").resolve()
    docs_output_path = docs_root_path.joinpath("../Documentation").resolve()

    config = AccelByteDocsBuilderConfig(docs_root_path)
    docs_builder = AccelByteDocsBuilder(config, docs_version)

    if docs_builder.build(docs_type, docs_output_path):
        return 0
    else:
        return -1


if __name__ == '__main__':
    sys.exit(main())
