import clr
clr.AddReference("jinja2")

import os, traceback, sys
from jinja2 import Environment, FileSystemLoader, Template


def get_environment(template_root, options=None):
    return Environment(loader=FileSystemLoader(template_root))


def render_template(environment, template_name, generic_context):
        context = {item.Key: item.Value for item in generic_context} if generic_context else {}

        template = environment.get_template(template_name)
        
        return template.render(**context)


def render_template_string(template_source, generic_context):
    context = {item.Key: item.Value for item in generic_context} if generic_context else {}

    template = Template(template_source)

    return template.render(**context)
