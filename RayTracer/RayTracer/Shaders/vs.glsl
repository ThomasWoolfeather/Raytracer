in vec2 Position;
in vec3 Color;
out vec3 oColor;
void main()
{
  oColor = Color;
  gl_Position = vec4(position.x, position.y, 0, 1);
}