import sys
import xml.etree.ElementTree as element_tree
from pathlib import Path
from typing import List


def retrieve_version_from_project(project: Path) -> str:
    xml = element_tree.parse(project)
    root = xml.getroot()

    version = root.find(".//Version").text

    print(f"Found version: '{version}' in project file")

    return version


def append_to_target_name(target: Path, data: List[str], delimiter: str):
    ## Insert the append data after the name but before the extension
    updated_name = f"{target.stem}{delimiter}{delimiter.join(data)}{target.suffix}"

    ## Replace instead of rename because Windows can't handle the file existing.
    target.replace(target.parent.joinpath(updated_name))

    print(f"Successfully renamed artifact '{target.name}' to '{updated_name}'")


def main(args: list):
    print(args)
    assert (len(args) >= 3)

    project = Path(args[1])
    target = Path(args[2])
    delimiter = args[3] if len(args) > 3 else '_'

    version = retrieve_version_from_project(project)
    append_to_target_name(target, [version], delimiter)


main(sys.argv)
