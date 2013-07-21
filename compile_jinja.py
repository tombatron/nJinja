import os
import clr


def get_complete_file_list(root_directory):
    walk_result = os.walk(root_directory)

    return [os.path.join(dp, f) for dp, dn, fns in walk_result for f in fns]


base_directory = os.path.dirname(__file__)

jinja2_src_directory = os.path.join(base_directory, 'jinja2_package/jinja2')
dependencies_src_directory = os.path.join(base_directory, 'jinja2_package/dependencies')

all_files = get_complete_file_list(jinja2_src_directory) + get_complete_file_list(dependencies_src_directory)

jinja2_bin_path = os.path.join(os.path.dirname(__file__), 'jinja2_package/jinja2.dll')

clr.CompileModules(jinja2_bin_path, *all_files)
