using MediatR;
using Valetax.Application.Features.Trees.Queries.GetTree;

namespace Valetax.Application.Features.Trees.Commands.GetOrCreateTree;

public sealed record GetOrCreateTreeCommand(string TreeName) : IRequest<TreeNodeDto?>;
