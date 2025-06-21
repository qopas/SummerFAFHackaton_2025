#!/bin/bash

set -e

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

IMAGE_NAME="${1:-ediki-api}"
TAG="${2:-latest}"
REGISTRY="${3:-}"

echo -e "${GREEN}🚀 Building multi-architecture Docker images for ${IMAGE_NAME}:${TAG}${NC}"

if ! docker buildx ls | grep -q "multiarch-builder"; then
    echo -e "${YELLOW}📝 Creating multi-architecture builder...${NC}"
    docker buildx create --name multiarch-builder --use
    docker buildx inspect --bootstrap
fi

docker buildx use multiarch-builder

echo -e "${GREEN}🔨 Building API image for linux/amd64,linux/arm64...${NC}"
if [ -n "$REGISTRY" ]; then
    FULL_IMAGE_NAME="$REGISTRY/$IMAGE_NAME"
else
    FULL_IMAGE_NAME="$IMAGE_NAME"
fi

docker buildx build \
    --platform linux/amd64,linux/arm64 \
    --tag "$FULL_IMAGE_NAME:$TAG" \
    --tag "$FULL_IMAGE_NAME:latest" \
    --file Dockerfile \
    ${REGISTRY:+--push} \
    .

# Build migrations image
echo -e "${GREEN}🔨 Building migrations image for linux/amd64,linux/arm64...${NC}"
MIGRATIONS_IMAGE_NAME="$FULL_IMAGE_NAME-migrations"

docker buildx build \
    --platform linux/amd64,linux/arm64 \
    --tag "$MIGRATIONS_IMAGE_NAME:$TAG" \
    --tag "$MIGRATIONS_IMAGE_NAME:latest" \
    --file Dockerfile.migrations \
    ${REGISTRY:+--push} \
    .

echo -e "${GREEN}✅ Multi-architecture build completed!${NC}"
echo -e "${YELLOW}📋 Built images:${NC}"
echo -e "   • $FULL_IMAGE_NAME:$TAG"
echo -e "   • $MIGRATIONS_IMAGE_NAME:$TAG"

if [ -n "$REGISTRY" ]; then
    echo -e "${GREEN}🚢 Images pushed to registry: $REGISTRY${NC}"
else
    echo -e "${YELLOW}💡 To push to a registry, run:${NC}"
    echo -e "   ./build-multiarch.sh $IMAGE_NAME $TAG <your-registry>"
    echo -e "${YELLOW}💡 For example with Docker Hub:${NC}"
    echo -e "   ./build-multiarch.sh $IMAGE_NAME $TAG smeloved"
fi

echo -e "${GREEN}🎯 Ready for AWS deployment on both x86_64 and ARM64 instances!${NC}" 