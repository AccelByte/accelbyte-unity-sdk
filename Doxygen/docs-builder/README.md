# Docs Builder

This is a tool to automate documentation build in any `C++` or `C#` project. This tool use [doxygen](https://www.doxygen.nl/manual/install.html) as the generator of HTML. Because the [doxygen](https://www.doxygen.nl/manual/install.html) support for markdown is not fully optimized at the moment the script also pre-process the markdown files before passing it to [doxygen](https://www.doxygen.nl/manual/install.html). Output from [doxygen](https://www.doxygen.nl/manual/install.html) also post-processed to fix some HTML formating issues that [doxygen](https://www.doxygen.nl/manual/install.html) has when processing markdown files. Styling of the HTML document is done using [doxygen-awesome-css](https://github.com/jothepro/doxygen-awesome-css).

## Requirements
To start integrating this with your project, following application should be installed on your machine:

- [doxygen](https://www.doxygen.nl/manual/install.html) at least version [1.9.5](https://www.doxygen.nl/download.html)
- [python 3.11](https://www.python.org/downloads/)
- [doxygen-awesome-css](https://github.com/jothepro/doxygen-awesome-css)

## How to Set Docs Builder in a Project

1. From the root of your project add this repository as a submodule using following command

```bash
git submodule add git@bitbucket.org:accelbyte/docs-builder.git Doxygen/docs-builder
```

2. Copy `docs_config.json.sample` to `Doxygen` folder and rename it to `docs_config.json.sample`
3. Copy `Doxyfile.in.sample` to `Doxygen` folder and rename it to `Doxyfile.in`
4. Create `Style` folder in `Doxygen` directory and add your `css` files to control the looks of HTML. [doxygen-awesome-css](https://github.com/jothepro/doxygen-awesome-css) has some option for styling or you can write your own. Copy any `css` files that you want to use to this folder and specify the file in `docs_config.json`
4. Edit `doxyfile_static` entry in `docs_config.json` to reflect your project information. This is one of the example. Every entry in `INPUT` field is a path relative to `Doxygen` folder. Do not edit any key in `json` document as it is used by the script (for example do not rename `doxyfile_static` or anything with `@*@` pattern).

```json
{
    "doxyfile_static" : {
        "@AUTHOR@": "AccelByte Inc.",
        "@COPYRIGHT@": "2022, AccelByte Inc.",
        "@PROJECT_NAME@": "AccelByte Cloud SDK",
        "@OUTPUT_DIRECTORY@": "build",
        "@HTML_OUTPUT@": "html",
        "@INPUT@": [
            "../Source/AccelByteUe4Sdk",
            "../README.md",
            "../CHANGELOG.md"],
        "@HTML_EXTRA_STYLESHEET@": [
            "Style/doxygen-awesome.css",
            "Style/doxygen-awesome-sidebar-only-darkmode-toggle.css"
        ]
    }
}
```